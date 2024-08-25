using AutoMapper;
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
