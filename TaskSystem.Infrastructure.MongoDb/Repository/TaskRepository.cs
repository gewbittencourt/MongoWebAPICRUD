using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSystem.Domain.Entities;
using TaskSystem.Domain.Interfaces;
using TaskSystem.Service.DTO;

namespace TaskSystem.Infrastructure.MongoDb.Repository
{
	public class TaskRepository : ITaskRepository
	{

		private readonly IMongoCollection<Tasks> _tasks;
		private readonly IMapper _mapper;


		public TaskRepository(IMapper mapper, IMongoClient client)
		{
			_mapper = mapper;
			var database = client.GetDatabase("TaskSystem");
			var collections = database.GetCollection<Tasks>(nameof(Tasks));
			_tasks = collections;
		}



		public async Task<Tasks> CreateNewTask(Tasks tasks, CancellationToken cancellationToken)
		{
			tasks.NewTask(Guid.NewGuid());
			await _tasks.InsertOneAsync(tasks, cancellationToken);
			return tasks;

		}

		public async Task<bool> DeleteTask(Guid id, CancellationToken cancellationToken)
		{
			var filter = Builders<Tasks>.Filter.Eq(x => x.Id, id);
			var result = await _tasks.DeleteOneAsync(filter, cancellationToken);

			return result.DeletedCount == 1;
		}



		public async Task<IEnumerable<Tasks>> GetAllTasks(CancellationToken cancellationToken)
		{
			var tasks = await _tasks.Find(_ => true).ToListAsync(cancellationToken);
			return tasks;
		}



		public async Task<Tasks> GetDetailedTask(Guid id, CancellationToken cancellationToken)
		{
			var filter = Builders<Tasks>.Filter.Eq(x => x.Id, id);
			var task = await _tasks.Find(filter).FirstOrDefaultAsync(cancellationToken);
			return task;
		}





		public async Task<bool> UpdateTask(Guid id, Tasks task, CancellationToken cancellationToken)
		{
			var filter = Builders<Tasks>.Filter.Eq(x => x.Id, id);
			var taskFromSearch = await _tasks.Find(filter).FirstOrDefaultAsync(cancellationToken);

			var update = Builders<Tasks>.Update
				.Set(x => x.Title, string.IsNullOrEmpty(task.Title) ? taskFromSearch.Title : task.Title)
				.Set(x => x.Description, string.IsNullOrEmpty(task.Description) ? taskFromSearch.Description : task.Description)
				.Set(x => x.CreationDate, taskFromSearch.CreationDate);


			var result = await _tasks.UpdateOneAsync(filter, update, null, cancellationToken);

			return result.ModifiedCount == 1;
		}

		public async Task<bool> CompletedTask (Guid id, CancellationToken cancellationToken)
		{
			var filter = Builders<Tasks>.Filter.Eq(x => x.Id, id);
			var update = Builders<Tasks>.Update
				.Set(x => x.CompletationDate, DateTime.Now);

			var result = await _tasks.UpdateOneAsync(filter, update, null, cancellationToken);
			return result.ModifiedCount == 1;
		}
	}
}
