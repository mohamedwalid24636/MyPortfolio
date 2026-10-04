using AutoMapper;
using Domain.Models;
using Service.Attachments;
using Shared.DTOs;

namespace Service.MappingProfiles
{
    public class SocialLinkProfile : AttachmentProfile<SocialLink, SocialLinkDto>
    {
        public SocialLinkProfile(AttachmentUrls attachmentUrls) : base(attachmentUrls)
        {
            CreateMap<SocialLink, SocialLinkDto>()
                .ForMember(d => d.IconUrl, o => o.MapFrom(AttachmentUrlFor(d => d.IconUrl)));

            CreateMap<CreateSocialLinkDto, SocialLink>()
                .ForMember(d => d.IconUrl, o => o.Ignore());

            CreateMap<UpdateSocialLinkDto, SocialLink>()
                .ForMember(d => d.IconUrl, o => o.Ignore());
        }
    }
}