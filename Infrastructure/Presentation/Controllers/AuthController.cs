using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTOs;

namespace Presentation.Controllers
{
    /// <summary>
    /// The only unauthenticated write in the API. Everything else is admin-only by default, because
    /// every other controller carries <c>[Authorize]</c>.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// Exchanges admin credentials for a bearer token.
        ///
        /// A wrong email and a wrong password are answered identically, with a bare 401 and no body,
        /// so the endpoint cannot be used to find out which addresses have an account. Any other
        /// failure is reported as 400 by the framework's model validation.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto dto)
        {
            var result = await authService.LoginAsync(dto);

            return result is null ? Unauthorized() : Ok(result);
        }
    }
}
