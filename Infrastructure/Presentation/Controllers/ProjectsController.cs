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
    public class ProjectsController(IProjectService projectService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<ProjectDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await projectService.GetAllAsync(parameters));

        [AllowAnonymous]
        [HttpGet("featured")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetFeatured()
            => Ok(await projectService.GetFeaturedAsync());

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            var project = await projectService.GetByIdAsync(id);
            return project is null ? NotFound() : Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create([FromForm] CreateProjectDto dto)
        {
            var project = await projectService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateProjectDto dto)
            => await projectService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await projectService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
