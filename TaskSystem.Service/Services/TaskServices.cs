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

		public Task<bool> CompletedTask(Guid id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public async Task<TasksDTO> CreateNewTask(TasksDTO tasksDTO, CancellationToken cancellationToken)
		{
			var task = new Tasks(title: tasksDTO.Title, description: tasksDTO.Description);
			var addCheck = await _taskRepository.CreateNewTask(task, cancellationToken);
			if (addCheck != null)
			{
				var result = _mapper.Map<TasksDTO>(addCheck);
				return result;
			}

			throw new NotImplementedException();

		}

		public Task<bool> DeleteTask(Guid id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<TasksDTO>> GetAllTasks(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<TasksDTO> GetDetailedTask(Guid id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateTask(Guid id, TasksDTO tasksDTO, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
