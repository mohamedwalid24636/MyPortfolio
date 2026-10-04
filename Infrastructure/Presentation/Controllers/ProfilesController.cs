using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTOs;
using Shared.DTOs.Common;

namespace Presentation.Controllers
{
    // Closed by default: every action here needs a token from /api/auth/login. Only the reads the
    // public portfolio renders are marked [AllowAnonymous], so anything added later stays protected
    // until someone deliberately opens it.
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController(IProfileService profileService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<ProfileDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await profileService.GetAllAsync(parameters));

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProfileDto>> GetById(int id)
        {
            var profile = await profileService.GetByIdAsync(id);
            return profile is null ? NotFound() : Ok(profile);
        }

        [HttpPost]
        public async Task<ActionResult<ProfileDto>> Create([FromForm] CreateProfileDto dto)
        {
            var profile = await profileService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = profile.Id }, profile);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateProfileDto dto)
            => await profileService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await profileService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
