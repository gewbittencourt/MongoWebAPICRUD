using TaskSystem.Domain.Entities;

namespace TaskSystem.Domain.Interfaces
{
	public interface ITaskRepository
	{

		Task<IEnumerable<Tasks>> GetAllTasks(CancellationToken cancellationToken);

		Task<Tasks> GetDetailedTask(Guid id, CancellationToken cancellationToken);

		Task CreateNewTask(Tasks tasks, CancellationToken cancellationToken);

		Task<bool> UpdateTask(Tasks task, CancellationToken cancellationToken);

		Task<bool> DeleteTask(Guid id, CancellationToken cancellationToken);





	}
}
