using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSystem.Application.Input;
using TaskSystem.Application.Output;

namespace TaskSystem.Application.Interface
{
	public interface ITaskService
	{
		Task<IEnumerable<GetTaskOutput>> GetAllTasks(CancellationToken cancellationToken);

		Task<GetTaskOutput> GetDetailedTask(Guid id, CancellationToken cancellationToken);

		Task<Guid> CreateNewTask(CreateTaskInput taskInput, CancellationToken cancellationToken);

		Task<bool> UpdateTask(Guid id, CreateTaskInput taskInput, CancellationToken cancellationToken);

		Task<bool> DeleteTask(Guid id, CancellationToken cancellationToken);

		Task<bool> CompleteTask(Guid id, CancellationToken cancellationToken);
	}
}
