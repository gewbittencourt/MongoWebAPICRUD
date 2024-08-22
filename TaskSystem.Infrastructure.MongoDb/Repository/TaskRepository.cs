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
using TaskSystem.Service.DTO;

namespace TaskSystem.Infrastructure.MongoDb.Repository
{
	public class TaskRepository : ITaskRepository
	{

		private readonly IMongoCollection<TaskCollection> _tasks;
		private readonly IMapper _mapper;


		/*public TaskRepository(IMapper mapper, IMongoClient client)
		{
			// O auto mapper está sendo utilizado para algo? o correto seria ele ser utilizado para mapear do objeto de domínio Task p/ o objeto TaskCollection
			_mapper = mapper;
			// essas configurações do client/collection você pode fazer na injeção de depência, podendo remover do contrutor da classe
			var database = client.GetDatabase("TaskSystem");
			var collections = database.GetCollection<Tasks>(nameof(Tasks));
			_tasks = collections;
		}*/

		//FEITO?

		public TaskRepository(IMongoCollection<TaskCollection> tasks, IMapper mapper)
		{
			_tasks = tasks;
			_mapper = mapper;
		}



		// retornar bool caso criado com sucesso
		//Feito
		public async Task CreateNewTask(Tasks tasks, CancellationToken cancellationToken)
		{
			await _tasks.InsertOneAsync(tasks,new InsertOneOptions(), cancellationToken);

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




		// Seu obj Task já tem um ID, não faz sentido ele receber o GUID como parametro
		//Feito?
		public async Task<bool> UpdateTask(Tasks task, CancellationToken cancellationToken)
		{

			var filter = Builders<Tasks>.Filter.Eq(x => x.Id, task.Id);

			var update = Builders<Tasks>.Update
				.Set(x => x.Title, task.Title)
				.Set(x => x.Description, task.Description)
				.Set(x => x.CompletationDate, task.CompletationDate)
				.Set(x => x.CreationDate, task.CreationDate);
			var result = await _tasks.UpdateOneAsync(filter, update, null, cancellationToken);

			return result.ModifiedCount == 1;
		}


	}
}
