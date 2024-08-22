using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace TaskSystem.Service.DTO
{
	public class TasksDTO
	{
		[JsonIgnore]
		public Guid Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }
		[JsonIgnore]
		public DateTime CreationDate { get; set; }
		[JsonIgnore]
		public DateTime CompletationDate { get; set; }
	}
}
