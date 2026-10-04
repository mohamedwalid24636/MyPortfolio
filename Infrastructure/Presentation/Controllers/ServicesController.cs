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
    public class ServicesController(IServiceService serviceService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<ServiceDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await serviceService.GetAllAsync(parameters));

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ServiceDto>> GetById(int id)
        {
            var service = await serviceService.GetByIdAsync(id);
            return service is null ? NotFound() : Ok(service);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceDto>> Create([FromForm] CreateServiceDto dto)
        {
            var service = await serviceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateServiceDto dto)
            => await serviceService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await serviceService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
