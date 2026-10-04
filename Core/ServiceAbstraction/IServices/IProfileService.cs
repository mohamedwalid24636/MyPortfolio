using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface IProfileService : IGenericService<ProfileDto, CreateProfileDto, UpdateProfileDto, int>
    {
    }
}
