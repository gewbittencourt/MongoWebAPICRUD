using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSystem.Domain.Entities;
using TaskSystem.Service.DTO;

namespace TaskSystem.Infrastructure.MongoDb.Mapper
{
	public class MappingClass : Profile
	{

		public MappingClass()
		{

			CreateMap<Tasks, TasksDTO>().ReverseMap();


		}

	}
}
