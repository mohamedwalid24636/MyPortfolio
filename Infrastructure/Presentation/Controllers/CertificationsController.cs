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
    public class CertificationsController(ICertificationService certificationService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<CertificationDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await certificationService.GetAllAsync(parameters));

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CertificationDto>> GetById(int id)
        {
            var certification = await certificationService.GetByIdAsync(id);
            return certification is null ? NotFound() : Ok(certification);
        }

        [HttpPost]
        public async Task<ActionResult<CertificationDto>> Create([FromForm] CreateCertificationDto dto)
        {
            var certification = await certificationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = certification.Id }, certification);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateCertificationDto dto)
            => await certificationService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await certificationService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
