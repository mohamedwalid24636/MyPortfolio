using AutoMapper;
using Domain.Models;
using Service.Attachments;
using Shared.DTOs;

namespace Service.MappingProfiles
{
    public class ProjectImageProfile : AttachmentProfile<ProjectImage, ProjectImageDto>
    {
        public ProjectImageProfile(AttachmentUrls attachmentUrls) : base(attachmentUrls)
        {
            CreateMap<ProjectImage, ProjectImageDto>()
                .ForMember(d => d.ImageUrl, o => o.MapFrom(AttachmentUrlFor(d => d.ImageUrl)));

            CreateMap<CreateProjectImageDto, ProjectImage>()
                .ForMember(d => d.ImageUrl, o => o.Ignore());

            CreateMap<UpdateProjectImageDto, ProjectImage>()
                .ForMember(d => d.ImageUrl, o => o.Ignore());
        }
    }
}