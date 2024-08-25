using AutoMapper;
using TaskSystem.Domain.Entities;
using TaskSystem.Infrastructure.MongoDb.Collection;

namespace TaskSystem.Infrastructure.MongoDb.Mapper
{
	public class MappingCollection : Profile
	{

		public MappingCollection()
		{
			CreateMap<Tasks, TaskCollection>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.TaskID, opt => opt.MapFrom(src => src.Id));

			CreateMap<TaskCollection, Tasks>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TaskID));
		}
	}
}
