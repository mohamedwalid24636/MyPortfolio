using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTOs;
using Shared.DTOs.Common;

namespace Presentation.Controllers
{
    // The admin inbox. Reading, editing and deleting messages is admin-only, and the class-level
    // [Authorize] is what guarantees that. Creating a message is the single exception, because the
    // public contact form has to be able to post without a token.
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactMessagesController(IContactMessageService contactMessageService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginationResult<ContactMessageDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await contactMessageService.GetAllAsync(parameters));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ContactMessageDto>> GetById(int id)
        {
            var message = await contactMessageService.GetByIdAsync(id);
            return message is null ? NotFound() : Ok(message);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ContactMessageDto>> Create([FromBody] CreateContactMessageDto dto)
        {
            var message = await contactMessageService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = message.Id }, message);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateContactMessageDto dto)
            => await contactMessageService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpPatch("{id:int}/read")]
        public async Task<ActionResult> MarkAsRead(int id)
            => await contactMessageService.MarkAsReadAsync(id) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await contactMessageService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
