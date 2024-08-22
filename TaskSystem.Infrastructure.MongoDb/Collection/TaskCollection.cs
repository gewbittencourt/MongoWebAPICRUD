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
		public const string CollectionName =  "TaskCollection";

		[BsonId]
		[BsonIgnoreIfDefault]
		public ObjectId Id { get; set; }

		[BsonElement("TaskTitle")]
		public string Title { get; set; }

		[BsonElement("TaskDescription")]
		public string Description { get; set; }

		[BsonElement("CreationDate")]
		public DateTime CreationDate { get; set; }


		[BsonElement("CompletationDate")]
		public DateTime CompletationDate { get; set; }
	}

}
