using AutoMapper;
using Domain.Models;
using Service.Attachments;
using Shared.DTOs;

namespace Service.MappingProfiles
{
    public class EducationProfile : AttachmentProfile<Education, EducationDto>
    {
        public EducationProfile(AttachmentUrls attachmentUrls) : base(attachmentUrls)
        {
            CreateMap<Education, EducationDto>()
                .ForMember(d => d.InstitutionLogoUrl, o => o.MapFrom(AttachmentUrlFor(d => d.InstitutionLogoUrl)));

            CreateMap<CreateEducationDto, Education>()
                .ForMember(d => d.InstitutionLogoUrl, o => o.Ignore());

            CreateMap<UpdateEducationDto, Education>()
                .ForMember(d => d.InstitutionLogoUrl, o => o.Ignore());
        }
    }
}