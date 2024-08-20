using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSystem.Service.DTO;

namespace TaskSystem.Service.Interface
{
	public interface ITaskService
	{

		Task<IEnumerable<TasksDTO>> GetAllTasks(CancellationToken cancellationToken);

		Task<TasksDTO> GetDetailedTask(Guid id, CancellationToken cancellationToken);

		Task<TasksDTO> CreateNewTask(TasksDTO tasksDTO, CancellationToken cancellationToken);

		Task<bool> UpdateTask(Guid id, TasksDTO tasksDTO, CancellationToken cancellationToken);

		Task<bool> DeleteTask(Guid id, CancellationToken cancellationToken);

		Task<bool> CompleteTask(Guid id, CancellationToken cancellationToken);


	}
}
