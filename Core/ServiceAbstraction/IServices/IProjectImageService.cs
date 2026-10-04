using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface IProjectImageService : IGenericService<ProjectImageDto, CreateProjectImageDto, UpdateProjectImageDto, int>
    {
    }
}
