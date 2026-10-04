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
    public class TechnologiesController(ITechnologyService technologyService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<TechnologyDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await technologyService.GetAllAsync(parameters));

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TechnologyDto>> GetById(int id)
        {
            var technology = await technologyService.GetByIdAsync(id);
            return technology is null ? NotFound() : Ok(technology);
        }

        [HttpPost]
        public async Task<ActionResult<TechnologyDto>> Create([FromForm] CreateTechnologyDto dto)
        {
            var technology = await technologyService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = technology.Id }, technology);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateTechnologyDto dto)
            => await technologyService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await technologyService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
