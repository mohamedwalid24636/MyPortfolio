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
    public class SocialLinksController(ISocialLinkService socialLinkService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<SocialLinkDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await socialLinkService.GetAllAsync(parameters));

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SocialLinkDto>> GetById(int id)
        {
            var socialLink = await socialLinkService.GetByIdAsync(id);
            return socialLink is null ? NotFound() : Ok(socialLink);
        }

        [HttpPost]
        public async Task<ActionResult<SocialLinkDto>> Create([FromForm] CreateSocialLinkDto dto)
        {
            var socialLink = await socialLinkService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = socialLink.Id }, socialLink);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateSocialLinkDto dto)
            => await socialLinkService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await socialLinkService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
