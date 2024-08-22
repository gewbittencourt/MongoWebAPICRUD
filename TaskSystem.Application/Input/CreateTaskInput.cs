using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskSystem.Application.Input
{
	public class CreateTaskInput
	{
		public string Title { get; set; }

		public string Description { get; set; }

	}
}
