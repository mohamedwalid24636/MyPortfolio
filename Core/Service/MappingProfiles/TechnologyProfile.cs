using AutoMapper;
using Domain.Models;
using Service.Attachments;
using Shared.DTOs;

namespace Service.MappingProfiles
{
    public class TechnologyProfile : AttachmentProfile<Technology, TechnologyDto>
    {
        public TechnologyProfile(AttachmentUrls attachmentUrls) : base(attachmentUrls)
        {
            CreateMap<Technology, TechnologyDto>()
                .ForMember(d => d.IconUrl, o => o.MapFrom(AttachmentUrlFor(d => d.IconUrl)));

            CreateMap<CreateTechnologyDto, Technology>()
                .ForMember(d => d.IconUrl, o => o.Ignore());

            CreateMap<UpdateTechnologyDto, Technology>()
                .ForMember(d => d.IconUrl, o => o.Ignore());
        }
    }
}