using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface IResumeService : IGenericService<ResumeDto, CreateResumeDto, UpdateResumeDto, int>
    {
        Task<ResumeDto?> GetActiveAsync();
    }
}
