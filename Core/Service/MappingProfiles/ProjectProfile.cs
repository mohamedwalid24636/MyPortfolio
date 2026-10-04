using AutoMapper;
using Domain.Models;
using Service.Attachments;
using Shared.DTOs;

namespace Service.MappingProfiles
{
    public class ProjectProfile : AttachmentProfile<Project, ProjectDto>
    {
        public ProjectProfile(AttachmentUrls attachmentUrls) : base(attachmentUrls)
        {
            CreateMap<Project, ProjectDto>()
                .ForMember(d => d.ImageUrl, o => o.MapFrom(AttachmentUrlFor(d => d.ImageUrl)))
                .ForMember(d => d.Images, o => o.MapFrom(s => s.ProjectImages ?? Enumerable.Empty<ProjectImage>()))
                .ForMember(d => d.Tags, o => o.MapFrom(s => s.Project_Tags == null
                    ? Enumerable.Empty<Tag>()
                    : s.Project_Tags.Where(pt => pt.Tag != null).Select(pt => pt.Tag).ToList()))
                .ForMember(d => d.Categories, o => o.MapFrom(s => s.Project_Categories == null
                    ? Enumerable.Empty<Category>()
                    : s.Project_Categories.Where(pc => pc.Category != null).Select(pc => pc.Category).ToList()))
                .ForMember(d => d.Technologies, o => o.MapFrom(s => s.Project_Technologies == null
                    ? Enumerable.Empty<Technology>()
                    : s.Project_Technologies.Where(pt => pt.Technology != null).Select(pt => pt.Technology).ToList()));

            CreateMap<CreateProjectDto, Project>()
                .ForMember(d => d.ImageUrl, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.ProjectImages, o => o.Ignore())
                .ForMember(d => d.Project_Tags, o => o.Ignore())
                .ForMember(d => d.Project_Categories, o => o.Ignore())
                .ForMember(d => d.Project_Technologies, o => o.Ignore());

            CreateMap<UpdateProjectDto, Project>()
                .ForMember(d => d.ImageUrl, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.ProjectImages, o => o.Ignore())
                .ForMember(d => d.Project_Tags, o => o.Ignore())
                .ForMember(d => d.Project_Categories, o => o.Ignore())
                .ForMember(d => d.Project_Technologies, o => o.Ignore());
        }
    }
}
