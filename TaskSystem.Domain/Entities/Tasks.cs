using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TaskSystem.Domain.Entities

{
	// Aqui vc está lidando com uma collection, o correto é a mesma ficar dentro do projeto mongoDb. Ex: Infra.MongoDb.Collection.TaskColection.cs
	// Uma vez que vc mapeou sua colection, o correto é vc ter uma classe de domínio chamada Tasks 
	public class Tasks
	{
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		[BsonElement("ID")]
		public Guid Id { get; private set; }

		[BsonElement("TaskTitle")]
		public string Title { get; private set; }

		[BsonElement("TaskDescription")]
		public string Description { get; private set; }

		[BsonElement("CreationDate")]
		public DateTime CreationDate { get; private set; }


		[BsonElement("CompletationDate")]
		public DateTime CompletationDate { get; private set; }



		public Tasks(string title, string description)
		{
			Title = title;
			Description = description;
			CreationDate = DateTime.Now;
		}

		//PRECISA?
		public void NewTask(Guid id) => Id = id;

		public void UpdateTitle(string title) => Title = title;


		public void UpdateDescription(string description) => Description = description;

	}
}
