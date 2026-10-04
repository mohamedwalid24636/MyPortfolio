using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface ISocialLinkService : IGenericService<SocialLinkDto, CreateSocialLinkDto, UpdateSocialLinkDto, int>
    {
    }
}
