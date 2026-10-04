using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface ITechnologyService : IGenericService<TechnologyDto, CreateTechnologyDto, UpdateTechnologyDto, int>
    {
    }
}
