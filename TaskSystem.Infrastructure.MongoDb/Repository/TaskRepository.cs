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
	internal class TaskRepository : ITaskRepository
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
			tasks.
			await _tasks.InsertOneAsync(tasks);
			return tasks;
			
		}

		public Task<Tasks> DeleteTask(Guid id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Tasks>> GetAllTasks(CancellationToken cancellationToken)
		{
			var tasks = await _tasks.Find(_=>true).ToListAsync();
			return tasks;
		}

		public Task<Tasks> GetDetailedTask(Guid id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<Tasks> UpdateTask(Tasks tasks, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
