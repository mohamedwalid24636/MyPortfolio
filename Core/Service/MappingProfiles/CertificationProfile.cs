using AutoMapper;
using Domain.Models;
using Service.Attachments;
using Shared.DTOs;

namespace Service.MappingProfiles
{
    public class CertificationProfile : AttachmentProfile<Certification, CertificationDto>
    {
        public CertificationProfile(AttachmentUrls attachmentUrls) : base(attachmentUrls)
        {
            CreateMap<Certification, CertificationDto>()
                .ForMember(d => d.CertificateUrl, o => o.MapFrom(AttachmentUrlFor(d => d.CertificateUrl)));

            CreateMap<CreateCertificationDto, Certification>()
                .ForMember(d => d.CertificateUrl, o => o.Ignore());

            CreateMap<UpdateCertificationDto, Certification>()
                .ForMember(d => d.CertificateUrl, o => o.Ignore());
        }
    }
}