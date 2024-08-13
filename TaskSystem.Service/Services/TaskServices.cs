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

		public async Task<bool> CompletedTask(Guid id, CancellationToken cancellationToken)
		{
			var result = await _taskRepository.CompletedTask(id, cancellationToken);
			return result;

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

		public async Task<bool> DeleteTask(Guid id, CancellationToken cancellationToken)
		{
			var result = await _taskRepository.DeleteTask(id, cancellationToken);
			return result;
		}

		public async Task<IEnumerable<TasksDTO>> GetAllTasks(CancellationToken cancellationToken)
		{
			var result = await _taskRepository.GetAllTasks(cancellationToken);
			var returnList = _mapper.Map<IEnumerable<TasksDTO>>(result);
			return returnList;
		}

		public async Task<TasksDTO> GetDetailedTask(Guid id, CancellationToken cancellationToken)
		{
			var result = await _taskRepository.GetDetailedTask(id, cancellationToken);
			var returnObject = _mapper.Map<TasksDTO>(result);
			return returnObject;
		}

		public async Task<bool> UpdateTask(Guid id, TasksDTO tasksDTO, CancellationToken cancellationToken)
		{
			var tasks = _mapper.Map<Tasks>(tasksDTO);
			var result = await _taskRepository.UpdateTask(id, tasks, cancellationToken);
			return result;
		}




	}
}
