using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface IEducationService : IGenericService<EducationDto, CreateEducationDto, UpdateEducationDto, int>
    {
    }
}
