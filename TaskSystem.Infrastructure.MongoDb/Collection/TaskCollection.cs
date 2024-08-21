using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSystem.Infrastructure.MongoDb.Collection
{
	public class TaskCollection
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
	}
}
