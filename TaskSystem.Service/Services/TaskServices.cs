using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSystem.Service.DTO;
using TaskSystem.Service.Interface;
using TaskSystem.Domain.Interfaces;
using TaskSystem.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;


namespace TaskSystem.Service.Services
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

		public async Task<bool> CompleteTask(Guid id, CancellationToken cancellationToken)
		{
			try
			{
				var task = await _taskRepository.GetDetailedTask(id, cancellationToken);
				var complete = await _taskRepository.CompleteTask(task, cancellationToken);

				if (!complete)
				{
					_logger.LogError("A tarefa não pode ser concluída.");
					throw new InvalidOperationException("A tarefa não pode ser concluída.");
				}

				return true;
			}
			catch (OperationCanceledException ex)
			{
				_logger.LogWarning(ex, "A operação foi cancelada.");
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Um erro ocorreu ao tentar completar a tarefa.");
				return false;
			}
		}

		public async Task<Guid> CreateNewTask(TasksDTO tasksDTO, CancellationToken cancellationToken)
		{
			try
			{
				var task = _mapper.Map<Tasks>(tasksDTO);

				await _taskRepository.CreateNewTask(task, cancellationToken);

				return task.Id;

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
				throw;
			}
		}


		public async Task<bool> DeleteTask(Guid id, CancellationToken cancellationToken)
		{
			try
			{
				// usar nomes mais descritivo. Ex: deleted
				//feito
				var deletedTask = await _taskRepository.DeleteTask(id, cancellationToken);

				if (!deletedTask)
				{
					_logger.LogError("Erro ao deletar a tarefa. Ela pode não existir");
					throw new InvalidOperationException($"Falha ao deletar a tarefa com o ID {id}. A tarefa pode não existir.");
				}

				return deletedTask;
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



		public async Task<IEnumerable<TasksDTO>> GetAllTasks(CancellationToken cancellationToken)
		{
			try
			{
				var resultTask = await _taskRepository.GetAllTasks(cancellationToken);

				if (resultTask == null)
				{
					_logger.LogWarning("Nenhuma tarefa encontrada!");
					throw new InvalidOperationException("Nenhuma tarefa foi encontrada.");
				}

				var returnList = _mapper.Map<IEnumerable<TasksDTO>>(resultTask);
				return returnList;
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

		public async Task<TasksDTO> GetDetailedTask(Guid id, CancellationToken cancellationToken)
		{
			try
			{
				var resultTask = await _taskRepository.GetDetailedTask(id, cancellationToken);

				if (resultTask == null)
				{
					_logger.LogWarning("A tarefa não foi encontrada.");
					throw new KeyNotFoundException($"Tarefa com o ID {id} não foi encontrada.");
				}

				var returnDTO = _mapper.Map<TasksDTO>(resultTask);
				return returnDTO;
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


		public async Task<bool> UpdateTask(Guid id, TasksDTO tasksDTO, CancellationToken cancellationToken)
		{
			try
			{
				// O fluxo aqui poderia ficar:
				// 1- get na base pelo ID
				// 		a - Se encontrou: Atualizar o objeto utilizando o dto que recebeu
				//			Encaminhar request para o repositoru
				//		b - Se não encontrou: retornar erro

				//Feito?

				var tasks = await _taskRepository.GetDetailedTask(id, cancellationToken);
				if (tasks != null)
				{
					tasks.UpdateDescription(string.IsNullOrEmpty(tasksDTO.Title) ? tasks.Title : tasksDTO.Title);
					tasks.UpdateTitle(string.IsNullOrEmpty(tasksDTO.Description) ? tasks.Description : tasksDTO.Description);
				}
				else
				{
					_logger.LogWarning("Falha ao localizar a tarefa.");
					throw new InvalidOperationException($"Falha ao localizar a tarefa.");
				}

				var update = await _taskRepository.UpdateTask(tasks, cancellationToken);

				if (!update)
				{
					_logger.LogError("Falha ao atualizar a tarefa. A tarefa pode não existir ou os dados não foram salvos corretamente.");
					throw new InvalidOperationException($"Falha ao atualizar a tarefa com o ID {id}. A tarefa pode não existir ou os dados não foram salvos corretamente.");
				}

				return update;
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
	}
}
