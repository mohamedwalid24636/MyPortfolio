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
    public class ProjectImagesController(IProjectImageService projectImageService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<ProjectImageDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await projectImageService.GetAllAsync(parameters));

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProjectImageDto>> GetById(int id)
        {
            var image = await projectImageService.GetByIdAsync(id);
            return image is null ? NotFound() : Ok(image);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectImageDto>> Create([FromForm] CreateProjectImageDto dto)
        {
            var image = await projectImageService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = image.Id }, image);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateProjectImageDto dto)
            => await projectImageService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await projectImageService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
