using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface ITagService : IGenericService<TagDto, CreateTagDto, UpdateTagDto, int>
    {
    }
}
