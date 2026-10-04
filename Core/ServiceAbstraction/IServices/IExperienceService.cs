using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface IExperienceService : IGenericService<ExperienceDto, CreateExperienceDto, UpdateExperienceDto, int>
    {
    }
}
