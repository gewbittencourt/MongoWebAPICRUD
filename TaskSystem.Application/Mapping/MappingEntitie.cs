using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSystem.Application.Input;
using TaskSystem.Application.Output;
using TaskSystem.Domain.Entities;

namespace TaskSystem.Application.Mapping
{
	public class MappingEntitie : Profile
	{

		public MappingEntitie()
		{

			CreateMap<Tasks, GetTaskOutput>();

			CreateMap<CreateTaskInput, Tasks>().AfterMap((src, dest) => dest.NewTask());

		}
	}
}
