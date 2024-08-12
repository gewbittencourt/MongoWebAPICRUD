using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSystem.Domain.Entities;

namespace TaskSystem.Domain.Interfaces
{
	public interface ITaskRepository
	{

		Task<IEnumerable<Tasks>> GetAllTasks(CancellationToken cancellationToken);

		Task<Tasks> GetDetailedTask(Guid id, CancellationToken cancellationToken);

		Task<Tasks> CreateNewTask(Tasks tasks, CancellationToken cancellationToken);

		Task<bool> UpdateTask(Guid id, Tasks task, CancellationToken cancellationToken);

		Task<bool>	DeleteTask(Guid id, CancellationToken cancellationToken);

		Task<bool> CompletedTask(Guid id, CancellationToken cancellationToken);





	}
}
