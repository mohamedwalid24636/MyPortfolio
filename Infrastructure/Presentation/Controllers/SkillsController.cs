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
    public class SkillsController(ISkillService skillService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<SkillDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await skillService.GetAllAsync(parameters));

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SkillDto>> GetById(int id)
        {
            var skill = await skillService.GetByIdAsync(id);
            return skill is null ? NotFound() : Ok(skill);
        }

        // Form binding, not JSON: CreateSkillDto/UpdateSkillDto carry an IFormFile Icon (and the
        // IsDelete flag that retires it), exactly like every other attachment-owning controller.
        // [FromBody] rejects multipart with 415, which made the icon upload/replace/delete path in
        // SkillService unreachable.
        [HttpPost]
        public async Task<ActionResult<SkillDto>> Create([FromForm] CreateSkillDto dto)
        {
            var skill = await skillService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = skill.Id }, skill);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateSkillDto dto)
            => await skillService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await skillService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
