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
    public class ExperiencesController(IExperienceService experienceService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<ExperienceDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await experienceService.GetAllAsync(parameters));

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ExperienceDto>> GetById(int id)
        {
            var experience = await experienceService.GetByIdAsync(id);
            return experience is null ? NotFound() : Ok(experience);
        }

        [HttpPost]
        public async Task<ActionResult<ExperienceDto>> Create([FromForm] CreateExperienceDto dto)
        {
            var experience = await experienceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = experience.Id }, experience);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateExperienceDto dto)
            => await experienceService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await experienceService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
