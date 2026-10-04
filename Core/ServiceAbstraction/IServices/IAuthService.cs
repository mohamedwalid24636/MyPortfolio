using Shared.DTOs;

namespace ServiceAbstraction
{
    /// <summary>
    /// Turns admin credentials into a signed access token.
    ///
    /// <see cref="LoginAsync"/> answers <c>null</c> for every kind of bad credentials rather than
    /// describing which part was wrong, so the caller cannot be used to discover which email
    /// addresses have an account.
    /// </summary>
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto);
    }
}
