using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TaskSystem.Domain.Entities

{
	// Aqui vc está lidando com uma collection, o correto é a mesma ficar dentro do projeto mongoDb. Ex: Infra.MongoDb.Collection.TaskColection.cs
	// Uma vez que vc mapeou sua colection, o correto é vc ter uma classe de domínio chamada Tasks 
	public class Tasks
	{

		public Guid Id { get; private set; }

		public string Title { get; private set; }

		public string Description { get; private set; }

		public DateTime CreationDate { get; private set; }


		public DateTime CompletationDate { get; private set; }


			
		public Tasks(string title, string description)
		{
			Title = title;
			Description = description;

		}

		public void NewTask()
		{
			Id = Guid.NewGuid();
			CreationDate = DateTime.Now;
		} 

		public void Complete() => CompletationDate = DateTime.Now;

		public void UpdateTitle(string title) => Title = title;


		public void UpdateDescription(string description) => Description = description;

	}
}
