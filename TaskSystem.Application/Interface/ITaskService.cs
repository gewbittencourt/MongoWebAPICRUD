
using TaskSystem.Application.BaseResponse;
using TaskSystem.Application.Input;

namespace TaskSystem.Application.Interface
{
	public interface ITaskService
	{
		Task<BaseOutputApplication> GetAllTasks(CancellationToken cancellationToken);

		Task<BaseOutputApplication> GetDetailedTask(Guid id, CancellationToken cancellationToken);

		Task<BaseOutputApplication> CreateNewTask(CreateTaskInput taskInput, CancellationToken cancellationToken);

		Task<BaseOutputApplication> UpdateTask(Guid id, CreateTaskInput taskInput, CancellationToken cancellationToken);

		Task<BaseOutputApplication> DeleteTask(Guid id, CancellationToken cancellationToken);

		Task<BaseOutputApplication> CompleteTask(Guid id, CancellationToken cancellationToken);
	}
}
