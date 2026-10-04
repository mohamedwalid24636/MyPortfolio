using AutoMapper;
using Domain.Models;
using Service.Attachments;
using Shared.DTOs;

namespace Service.MappingProfiles
{
    public class ResumeProfile : AttachmentProfile<Resume, ResumeDto>
    {
        public ResumeProfile(AttachmentUrls attachmentUrls) : base(attachmentUrls)
        {
            CreateMap<Resume, ResumeDto>()
                .ForMember(d => d.FileUrl, o => o.MapFrom(AttachmentUrlFor(d => d.FileUrl)));

            // FileUrl / FileName / FileType / UploadedAt describe the uploaded bytes and are
            // produced by the service, never copied from the request.
            CreateMap<CreateResumeDto, Resume>()
                .ForMember(d => d.FileUrl, o => o.Ignore())
                .ForMember(d => d.FileName, o => o.Ignore())
                .ForMember(d => d.FileType, o => o.Ignore())
                .ForMember(d => d.UploadedAt, o => o.Ignore());

            CreateMap<UpdateResumeDto, Resume>()
                .ForMember(d => d.FileUrl, o => o.Ignore())
                .ForMember(d => d.FileName, o => o.Ignore())
                .ForMember(d => d.FileType, o => o.Ignore())
                .ForMember(d => d.UploadedAt, o => o.Ignore());
        }
    }
}
