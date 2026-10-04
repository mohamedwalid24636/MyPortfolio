using AutoMapper;
using Domain.Models;
using Service.Attachments;
using Shared.DTOs;

namespace Service.MappingProfiles
{
    public class BlogPostProfile : AttachmentProfile<BlogPost, BlogPostDto>
    {
        public BlogPostProfile(AttachmentUrls attachmentUrls) : base(attachmentUrls)
        {
            CreateMap<BlogPost, BlogPostDto>()
                .ForMember(d => d.CoverImageUrl, o => o.MapFrom(AttachmentUrlFor(d => d.CoverImageUrl)));

            // Slug, ReadingTime and CoverImageUrl are derived on the server, and the two dates
            // are timestamps, so none of them come from the request.
            CreateMap<CreateBlogPostDto, BlogPost>()
                .ForMember(d => d.Slug, o => o.Ignore())
                .ForMember(d => d.ReadingTime, o => o.Ignore())
                .ForMember(d => d.CoverImageUrl, o => o.Ignore())
                .ForMember(d => d.PublishedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore());

            CreateMap<UpdateBlogPostDto, BlogPost>()
                .ForMember(d => d.Slug, o => o.Ignore())
                .ForMember(d => d.ReadingTime, o => o.Ignore())
                .ForMember(d => d.CoverImageUrl, o => o.Ignore())
                .ForMember(d => d.PublishedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore());
        }
    }
}
