using AutoMapper;
using Service.Attachments;
using Shared.DTOs;
using ServiceEntity = Domain.Models.Service;

namespace Service.MappingProfiles
{
    public class ServiceProfile : AttachmentProfile<ServiceEntity, ServiceDto>
    {
        public ServiceProfile(AttachmentUrls attachmentUrls) : base(attachmentUrls)
        {
            CreateMap<ServiceEntity, ServiceDto>()
                .ForMember(d => d.IconUrl, o => o.MapFrom(AttachmentUrlFor(d => d.IconUrl)));

            CreateMap<CreateServiceDto, ServiceEntity>()
                .ForMember(d => d.IconUrl, o => o.Ignore());

            CreateMap<UpdateServiceDto, ServiceEntity>()
                .ForMember(d => d.IconUrl, o => o.Ignore());
        }
    }
}