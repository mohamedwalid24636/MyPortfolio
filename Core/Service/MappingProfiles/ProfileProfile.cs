using AutoMapper;
using Service.Attachments;
using Shared.DTOs;
using ProfileEntity = Domain.Models.Profile;

namespace Service.MappingProfiles
{
    public class ProfileProfile : AttachmentProfile<ProfileEntity, ProfileDto>
    {
        public ProfileProfile(AttachmentUrls attachmentUrls) : base(attachmentUrls)
        {
            CreateMap<ProfileEntity, ProfileDto>()
                .ForMember(d => d.ProfileImageUrl, o => o.MapFrom(AttachmentUrlFor(d => d.ProfileImageUrl)))
                .ForMember(d => d.AboutImageUrl, o => o.MapFrom(AttachmentUrlFor(d => d.AboutImageUrl)));

            CreateMap<CreateProfileDto, ProfileEntity>()
                .ForMember(d => d.ProfileImageUrl, o => o.Ignore())
                .ForMember(d => d.AboutImageUrl, o => o.Ignore());

            CreateMap<UpdateProfileDto, ProfileEntity>()
                .ForMember(d => d.ProfileImageUrl, o => o.Ignore())
                .ForMember(d => d.AboutImageUrl, o => o.Ignore());
        }
    }
}