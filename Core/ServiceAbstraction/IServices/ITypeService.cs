using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface ITypeService : IGenericService<TypeDto, CreateTypeDto, UpdateTypeDto, int>
    {
    }
}
