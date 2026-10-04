using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface ICertificationService : IGenericService<CertificationDto, CreateCertificationDto, UpdateCertificationDto, int>
    {
    }
}
