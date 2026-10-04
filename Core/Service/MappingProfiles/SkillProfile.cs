using AutoMapper;
using Domain.Models;
using Service.Attachments;
using Shared.DTOs;

namespace Service.MappingProfiles
{
    public class SkillProfile : AttachmentProfile<Skill, SkillDto>
    {
        public SkillProfile(AttachmentUrls attachmentUrls) : base(attachmentUrls)
        {
            CreateMap<Skill, SkillDto>()
                .ForMember(d => d.IconUrl, o => o.MapFrom(AttachmentUrlFor(d => d.IconUrl)))
                .ForMember(d => d.Types, o => o.MapFrom(s => s.Skill_Types == null
                    ? Enumerable.Empty<Domain.Models.Type>()
                    : s.Skill_Types.Where(st => st.Type != null).Select(st => st.Type).ToList()));

            CreateMap<CreateSkillDto, Skill>()
                .ForMember(d => d.IconUrl, o => o.Ignore())
                .ForMember(d => d.Skill_Types, o => o.Ignore());

            CreateMap<UpdateSkillDto, Skill>()
                .ForMember(d => d.IconUrl, o => o.Ignore())
                .ForMember(d => d.Skill_Types, o => o.Ignore());
        }
    }
}
