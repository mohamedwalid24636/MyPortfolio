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
    public class TypesController(ITypeService typeService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<TypeDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await typeService.GetAllAsync(parameters));

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TypeDto>> GetById(int id)
        {
            var type = await typeService.GetByIdAsync(id);
            return type is null ? NotFound() : Ok(type);
        }

        [HttpPost]
        public async Task<ActionResult<TypeDto>> Create([FromBody] CreateTypeDto dto)
        {
            var type = await typeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = type.Id }, type);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateTypeDto dto)
            => await typeService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await typeService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
