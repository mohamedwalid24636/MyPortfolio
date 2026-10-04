using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface IServiceService : IGenericService<ServiceDto, CreateServiceDto, UpdateServiceDto, int>
    {
    }
}
