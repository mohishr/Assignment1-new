using Assignment1.API.Models;
using AutoMapper;
using MongoDB.Bson;

namespace Assignment1.API
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Machine, MachineMongoModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ObjectId.GenerateNewId().ToString()));
            CreateMap<MachineMongoModel, Machine>();
        }
    }
}
