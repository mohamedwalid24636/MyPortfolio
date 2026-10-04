using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface ISkillService : IGenericService<SkillDto, CreateSkillDto, UpdateSkillDto, int>
    {
    }
}
