using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSystem.Service.DTO
{
	public class TasksDTO
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public DateTime CreationDate { get; set; }

		public DateTime CompletationDate { get; set; }
	}
}
