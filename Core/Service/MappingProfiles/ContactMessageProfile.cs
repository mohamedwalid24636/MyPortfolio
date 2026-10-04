using AutoMapper;
using Domain.Models;
using Shared.DTOs;

namespace Service.MappingProfiles
{
    public class ContactMessageProfile : AutoMapper.Profile
    {
        public ContactMessageProfile()
        {
            CreateMap<ContactMessage, ContactMessageDto>();
            CreateMap<CreateContactMessageDto, ContactMessage>()
                .ForMember(d => d.SentAt, o => o.Ignore())
                .ForMember(d => d.IsRead, o => o.Ignore());
            CreateMap<UpdateContactMessageDto, ContactMessage>();
        }
    }
}
