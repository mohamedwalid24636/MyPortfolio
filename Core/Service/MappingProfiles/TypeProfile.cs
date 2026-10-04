using AutoMapper;
using Domain.Models;
using Shared.DTOs;
using TypeEntity = Domain.Models.Type;

namespace Service.MappingProfiles
{
    public class TypeProfile : AutoMapper.Profile
    {
        public TypeProfile()
        {
            CreateMap<TypeEntity, TypeDto>();
            CreateMap<CreateTypeDto, TypeEntity>();
            CreateMap<UpdateTypeDto, TypeEntity>();
        }
    }
}
