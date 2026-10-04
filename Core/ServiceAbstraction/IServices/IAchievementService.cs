using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface IAchievementService : IGenericService<AchievementDto, CreateAchievementDto, UpdateAchievementDto, int>
    {
    }
}
