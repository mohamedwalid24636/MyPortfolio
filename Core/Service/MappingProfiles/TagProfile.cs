using AutoMapper;
using Domain.Models;
using Shared.DTOs;

namespace Service.MappingProfiles
{
    public class TagProfile : AutoMapper.Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDto>();
            CreateMap<CreateTagDto, Tag>();
            CreateMap<UpdateTagDto, Tag>();
        }
    }
}
