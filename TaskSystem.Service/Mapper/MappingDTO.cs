using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSystem.Domain.Entities;
using TaskSystem.Service.DTO;

namespace TaskSystem.Service.Mapper
{
	public class MappingDTO : Profile
	{

		public MappingDTO()
		{

			CreateMap<Tasks, TasksDTO>();


			//por conta disso deveria estar na camada de domain?
			CreateMap<TasksDTO, Tasks>().AfterMap((src, dest) => dest.NewTask());


		}

	}
}
