using AutoMapper;
using Domain.Models;
using Service.Attachments;
using Shared.DTOs;

namespace Service.MappingProfiles
{
    public class AchievementProfile : AttachmentProfile<Achievement, AchievementDto>
    {
        public AchievementProfile(AttachmentUrls attachmentUrls) : base(attachmentUrls)
        {
            CreateMap<Achievement, AchievementDto>()
                .ForMember(d => d.ImageUrl, o => o.MapFrom(AttachmentUrlFor(d => d.ImageUrl)));

            CreateMap<CreateAchievementDto, Achievement>()
                .ForMember(d => d.ImageUrl, o => o.Ignore());

            CreateMap<UpdateAchievementDto, Achievement>()
                .ForMember(d => d.ImageUrl, o => o.Ignore());
        }
    }
}