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


namespace TaskSystem.Service.Services
{
	public class TaskServices : ITaskService
	{
		private readonly ITaskRepository _taskRepository;
		private readonly IMapper _mapper;

		public TaskServices(ITaskRepository TaskRepository, IMapper mapper)
		{

			_taskRepository = TaskRepository;
			_mapper = mapper;

		}

		public async Task<bool> CompleteTask(Guid id, CancellationToken cancellationToken)
		{
			try
			{
				//Primeiro buscar pelo ID que recebeu, e encaminhar o objeto Task que retornou da base para a repository e o metodo complete tem a responsabilidade de altera-lo
				//feito?
				var task = await _taskRepository.GetDetailedTask(id, cancellationToken);
				var complete = await _taskRepository.CompleteTask(task, cancellationToken);

				if (!complete)
				{
					throw new InvalidOperationException("A tarefa não pode ser concluida.");
				}

				return true;
			}
			catch (OperationCanceledException ex)
			{
				Console.WriteLine($"A operação foi cancelada: {ex.Message}");
				throw;
			}
			catch (Exception ex)
			{
				// EStudar sobre ILogger p/ tratar suas mensagens através da interface da lib https://learn.microsoft.com/pt-br/dotnet/api/microsoft.extensions.logging.ilogger?view=net-8.0
				Console.WriteLine($"Um erro ocorreu: {ex.Message}");
				return false;
			}
		}

		public async Task<TasksDTO> CreateNewTask(TasksDTO tasksDTO, CancellationToken cancellationToken)
		{
			try
			{
				// Esse Mapeamento poderia estar dentro do automapper 
				var task = _mapper.Map<Tasks>(tasksDTO);
				//var task = new Tasks(title: tasksDTO.Title, description: tasksDTO.Description);


				// Esse Mapeamento poderia estar dentro do automapper 
				task.NewTask(Guid.NewGuid());

				var addResult = await _taskRepository.CreateNewTask(task, cancellationToken);

				if (addResult != null)
				{
					return tasksDTO;
				}

				throw new InvalidOperationException("Falha na criação da tarefa. Retorno nulo.");
			}
			catch (ArgumentNullException ex)
			{
				throw new ArgumentException("Um ou mais argumentos são nulos.", ex);
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException("A criação da task foi cancelada.", ex);
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Erro inesperado ao criar a task.", ex);
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
					throw new InvalidOperationException($"Falha ao deletar a tarefa com o ID {id}. A tarefa pode não existir.");
				}

				return deletedTask;
			}
			catch (ArgumentNullException ex)
			{
				throw new ArgumentException("O ID da tarefa fornecido é inválido.", ex);
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException("A exclusão da tarefa foi cancelada.", ex);
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Ocorreu um erro inesperado durante a exclusão da tarefa.", ex);
			}
		}



		public async Task<IEnumerable<TasksDTO>> GetAllTasks(CancellationToken cancellationToken)
		{
			try
			{
				var resultTask = await _taskRepository.GetAllTasks(cancellationToken);

				if (resultTask == null)
				{
					throw new InvalidOperationException("Nenhuma tarefa foi encontrada.");
				}

				var returnList = _mapper.Map<IEnumerable<TasksDTO>>(resultTask);
				return returnList;
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException("A operação de obtenção de tarefas foi cancelada.", ex);
			}
			catch (Exception ex)
			{
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
					throw new KeyNotFoundException($"Tarefa com o ID {id} não foi encontrada.");
				}

				var returnDTO = _mapper.Map<TasksDTO>(resultTask);
				return returnDTO;
			}


			catch (ArgumentNullException ex)
			{
				throw new ArgumentException("O ID da tarefa fornecido é inválido.", ex);
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException("A operação de obtenção dos detalhes da tarefa foi cancelada.", ex);
			}
			catch (KeyNotFoundException ex)
			{
				throw new KeyNotFoundException(ex.Message, ex);
			}
			catch (Exception ex)
			{
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
					throw new InvalidOperationException($"Falha ao localizar a tarefa.");
				}
				var update = await _taskRepository.UpdateTask(tasks, cancellationToken);

				if (!update)
				{
					throw new InvalidOperationException($"Falha ao atualizar a tarefa com o ID {id}. A tarefa pode não existir ou os dados não foram salvos corretamente.");
				}

				return update;
			}
			catch (ArgumentNullException ex)
			{
				throw new ArgumentException("O ID ou os dados da tarefa fornecidos são inválidos.", ex);
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException("A operação de atualização da tarefa foi cancelada.", ex);
			}
			catch (InvalidOperationException ex)
			{
				throw new InvalidOperationException(ex.Message, ex);
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Ocorreu um erro inesperado ao atualizar a tarefa.", ex);
			}
		}
	}
}
