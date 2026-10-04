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
    public class EducationsController(IEducationService educationService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<EducationDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await educationService.GetAllAsync(parameters));

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EducationDto>> GetById(int id)
        {
            var education = await educationService.GetByIdAsync(id);
            return education is null ? NotFound() : Ok(education);
        }

        [HttpPost]
        public async Task<ActionResult<EducationDto>> Create([FromForm] CreateEducationDto dto)
        {
            var education = await educationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = education.Id }, education);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateEducationDto dto)
            => await educationService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await educationService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
