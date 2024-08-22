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
using TaskSystem.Infrastructure.MongoDb.Collection;

namespace TaskSystem.Infrastructure.MongoDb.Repository
{
	public class TaskRepository : ITaskRepository
	{

		private readonly IMongoCollection<TaskCollection> _tasks;
		private readonly IMapper _mapper;


		public TaskRepository(IMongoCollection<TaskCollection> tasks, IMapper mapper)
		{
			_tasks = tasks;
			_mapper = mapper;
		}


		public async Task CreateNewTask(Tasks tasks, CancellationToken cancellationToken)
		{
			var taskCollection = _mapper.Map<TaskCollection>(tasks);
			await _tasks.InsertOneAsync(taskCollection, new InsertOneOptions(), cancellationToken);

		}

		public async Task<bool> DeleteTask(Guid id, CancellationToken cancellationToken)
		{
			var filter = Builders<TaskCollection>.Filter.Eq(x => x.TaskID, id);
			var result = await _tasks.DeleteOneAsync(filter, cancellationToken);

			return result.DeletedCount == 1;
		}



		public async Task<IEnumerable<Tasks>> GetAllTasks(CancellationToken cancellationToken)
		{
			var taskCollection = await _tasks.Find(_ => true).ToListAsync(cancellationToken);
			return _mapper.Map<IEnumerable<Tasks>>(taskCollection);
		}


		public async Task<Tasks> GetDetailedTask(Guid id, CancellationToken cancellationToken)
		{
			var filter = Builders<TaskCollection>.Filter.Eq(x => x.TaskID, id);
			var taskCollection = await _tasks.Find(filter).FirstOrDefaultAsync(cancellationToken);
			return _mapper.Map<Tasks>(taskCollection);
		}


		public async Task<bool> UpdateTask(Tasks task, CancellationToken cancellationToken)
		{
			var filter = Builders<TaskCollection>.Filter.Eq(x => x.TaskID, task.Id);

			var update = Builders<TaskCollection>.Update
				.Set(x => x.Title, task.Title)
				.Set(x => x.Description, task.Description)
				.Set(x => x.CompletationDate, task.CompletationDate)
				.Set(x => x.CreationDate, task.CreationDate);
			var result = await _tasks.UpdateOneAsync(filter, update, null, cancellationToken);

			return result.ModifiedCount == 1;
		}


	}
}
