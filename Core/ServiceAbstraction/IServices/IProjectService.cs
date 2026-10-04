using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface IProjectService : IGenericService<ProjectDto, CreateProjectDto, UpdateProjectDto, int>
    {
        Task<IEnumerable<ProjectDto>> GetFeaturedAsync();
    }
}
