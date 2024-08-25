using AutoMapper;
using Microsoft.Extensions.Logging;
using TaskSystem.Application.BaseResponse;
using TaskSystem.Application.Input;
using TaskSystem.Application.Interface;
using TaskSystem.Application.Output;
using TaskSystem.Domain.Entities;
using TaskSystem.Domain.Interfaces;

namespace TaskSystem.Application.Service
{
	public class TaskServices : ITaskService
	{
		private readonly ITaskRepository _taskRepository;
		private readonly IMapper _mapper;
		private readonly ILogger<TaskServices> _logger;

		public TaskServices(ITaskRepository TaskRepository, IMapper mapper, ILogger<TaskServices> logger)
		{

			_taskRepository = TaskRepository;
			_mapper = mapper;
			_logger = logger;
		}



		public async Task<BaseOutputApplication> CreateNewTask(CreateTaskInput createTask, CancellationToken cancellationToken)
		{
			try
			{
				var task = _mapper.Map<Tasks>(createTask);

				await _taskRepository.CreateNewTask(task, cancellationToken);

				return BaseOutputApplication.Success();
				//return task.Id;

			}
			catch (ArgumentNullException ex)
			{
				_logger.LogError(ex, "Um ou mais argumentos são nulos.");
				throw;
			}
			catch (OperationCanceledException ex)
			{
				_logger.LogWarning(ex, "A criação da task foi cancelada.");
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erro inesperado ao criar a task.");
				return BaseOutputApplication.Failure(ex);
				throw;
			}
		}


		public async Task<BaseOutputApplication> DeleteTask(Guid id, CancellationToken cancellationToken)
		{
			try
			{
				var deletedTask = await _taskRepository.DeleteTask(id, cancellationToken);

				if (!deletedTask)
				{
					_logger.LogError("Erro ao deletar a tarefa. Ela pode não existir");
					throw new InvalidOperationException($"Falha ao deletar a tarefa com o ID {id}. A tarefa pode não existir.");
				}

				return BaseOutputApplication.Success(deletedTask);
			}
			catch (ArgumentNullException ex)
			{
				_logger.LogError(ex, "Um ou mais elementos são nulos.");
				throw;
			}
			catch (OperationCanceledException ex)
			{
				_logger.LogWarning(ex, "A deleção da task foi cancelada.");
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erro inesperado ao deletar a task.");
				throw;
			}
		}



		public async Task<BaseOutputApplication> GetAllTasks(CancellationToken cancellationToken)
		{
			try
			{
				var resultTask = await _taskRepository.GetAllTasks(cancellationToken);
				var returnList = _mapper.Map<IEnumerable<GetTaskOutput>>(resultTask);
				return BaseOutputApplication.Success(returnList);
			}
			catch (OperationCanceledException ex)
			{
				_logger.LogError("A operação de obtenção de tarefas foi cancelada.");
				throw new OperationCanceledException("A operação de obtenção de tarefas foi cancelada.", ex);
			}
			catch (Exception ex)
			{
				_logger.LogError("Ocorreu um erro inesperado ao obter a lista de tarefas.");
				throw new ApplicationException("Ocorreu um erro inesperado ao obter a lista de tarefas.", ex);
			}
		}

		public async Task<BaseOutputApplication> GetDetailedTask(Guid id, CancellationToken cancellationToken)
		{
			try
			{
				var resultTask = await _taskRepository.GetDetailedTask(id, cancellationToken);

				if (resultTask == null)
				{
					_logger.LogWarning("A tarefa não foi encontrada.");
					throw new KeyNotFoundException($"Tarefa com o ID {id} não foi encontrada.");
				}

				var returnOutput = _mapper.Map<GetTaskOutput>(resultTask);
				return BaseOutputApplication.Success(returnOutput);
			}


			catch (ArgumentNullException ex)
			{
				_logger.LogError("O ID da tarefa fornecido é inválido.");
				throw new ArgumentException("O ID da tarefa fornecido é inválido.", ex);
			}
			catch (OperationCanceledException ex)
			{
				_logger.LogError("A operação de obtenção dos detalhes da tarefa foi cancelada");
				throw new OperationCanceledException("A operação de obtenção dos detalhes da tarefa foi cancelada.", ex);
			}
			catch (Exception ex)
			{
				_logger.LogError("Ocorreu um erro inesperado ao obter os detalhes da tarefa.");
				throw new ApplicationException("Ocorreu um erro inesperado ao obter os detalhes da tarefa.", ex);
			}
		}


		public async Task<BaseOutputApplication> UpdateTask(Guid id, CreateTaskInput taskInput, CancellationToken cancellationToken)
		{
			try
			{
				var tasks = await _taskRepository.GetDetailedTask(id, cancellationToken);
				if (tasks is null)
				{
					_logger.LogWarning("Falha ao localizar a tarefa.");
					throw new InvalidOperationException($"Falha ao localizar a tarefa.");
				}

				tasks.UpdateTask(string.IsNullOrEmpty(taskInput.Title) ? tasks.Title : taskInput.Title, string.IsNullOrEmpty(taskInput.Description) ? tasks.Description : taskInput.Description);
				var updated = await _taskRepository.UpdateTask(tasks, cancellationToken);

				if (!updated)
				{
					_logger.LogError("Falha ao atualizar a tarefa. A tarefa pode não existir ou os dados não foram salvos corretamente.");
					throw new InvalidOperationException($"Falha ao atualizar a tarefa com o ID {id}. A tarefa pode não existir ou os dados não foram salvos corretamente.");
				}

				return BaseOutputApplication.Success(updated);
			}
			catch (ArgumentNullException ex)
			{
				_logger.LogError("O ID ou os dados da tarefa fornecidos são inválidos.");
				throw new ArgumentException("O ID ou os dados da tarefa fornecidos são inválidos.", ex);
			}
			catch (OperationCanceledException ex)
			{
				_logger.LogError("A operação de atualização da tarefa foi cancelada.");
				throw new OperationCanceledException("A operação de atualização da tarefa foi cancelada.", ex);
			}
			catch (Exception ex)
			{
				_logger.LogError("Ocorreu um erro inesperado ao atualizar a tarefa.");
				throw new ApplicationException("Ocorreu um erro inesperado ao atualizar a tarefa.", ex);
			}
		}



		public async Task<BaseOutputApplication> CompleteTask(Guid id, CancellationToken cancellationToken)
		{
			try
			{
				var task = await _taskRepository.GetDetailedTask(id, cancellationToken);
				task.Complete();
				var updated = await _taskRepository.UpdateTask(task, cancellationToken);

				if (!updated)
				{
					_logger.LogError("A tarefa não pode ser concluída.");
					throw new InvalidOperationException("A tarefa não pode ser concluída.");
				}

				return BaseOutputApplication.Success(updated);
			}
			catch (OperationCanceledException ex)
			{
				_logger.LogWarning(ex, "A operação foi cancelada.");
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Um erro ocorreu ao tentar completar a tarefa.");
				throw;
			}
		}
	}
}


