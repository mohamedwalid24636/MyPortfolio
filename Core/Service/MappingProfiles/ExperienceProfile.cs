using AutoMapper;
using Domain.Models;
using Service.Attachments;
using Shared.DTOs;

namespace Service.MappingProfiles
{
    public class ExperienceProfile : AttachmentProfile<Experience, ExperienceDto>
    {
        public ExperienceProfile(AttachmentUrls attachmentUrls) : base(attachmentUrls)
        {
            CreateMap<Experience, ExperienceDto>()
                .ForMember(d => d.CompanyLogoUrl, o => o.MapFrom(AttachmentUrlFor(d => d.CompanyLogoUrl)));

            CreateMap<CreateExperienceDto, Experience>()
                .ForMember(d => d.CompanyLogoUrl, o => o.Ignore());

            CreateMap<UpdateExperienceDto, Experience>()
                .ForMember(d => d.CompanyLogoUrl, o => o.Ignore());
        }
    }
}