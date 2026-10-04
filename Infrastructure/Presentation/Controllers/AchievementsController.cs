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
    public class AchievementsController(IAchievementService achievementService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<AchievementDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await achievementService.GetAllAsync(parameters));

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AchievementDto>> GetById(int id)
        {
            var achievement = await achievementService.GetByIdAsync(id);
            return achievement is null ? NotFound() : Ok(achievement);
        }

        [HttpPost]
        public async Task<ActionResult<AchievementDto>> Create([FromForm] CreateAchievementDto dto)
        {
            var achievement = await achievementService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = achievement.Id }, achievement);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateAchievementDto dto)
            => await achievementService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await achievementService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
