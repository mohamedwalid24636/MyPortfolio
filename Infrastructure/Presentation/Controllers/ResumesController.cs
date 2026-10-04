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
    public class ResumesController(IResumeService resumeService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<ResumeDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await resumeService.GetAllAsync(parameters));

        [AllowAnonymous]
        [HttpGet("active")]
        public async Task<ActionResult<ResumeDto>> GetActive()
        {
            var resume = await resumeService.GetActiveAsync();
            return resume is null ? NotFound() : Ok(resume);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResumeDto>> GetById(int id)
        {
            var resume = await resumeService.GetByIdAsync(id);
            return resume is null ? NotFound() : Ok(resume);
        }

        [HttpPost]
        public async Task<ActionResult<ResumeDto>> Create([FromForm] CreateResumeDto dto)
        {
            var resume = await resumeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = resume.Id }, resume);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateResumeDto dto)
            => await resumeService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await resumeService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
