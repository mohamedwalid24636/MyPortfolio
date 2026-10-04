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
    public class BlogPostsController(IBlogPostService blogPostService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<BlogPostDto>>> GetAll([FromQuery] QueryParameters parameters)
            => Ok(await blogPostService.GetAllAsync(parameters));

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BlogPostDto>> GetById(int id)
        {
            var blogPost = await blogPostService.GetByIdAsync(id);
            return blogPost is null ? NotFound() : Ok(blogPost);
        }

        [HttpPost]
        public async Task<ActionResult<BlogPostDto>> Create([FromForm] CreateBlogPostDto dto)
        {
            var blogPost = await blogPostService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = blogPost.Id }, blogPost);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateBlogPostDto dto)
            => await blogPostService.UpdateAsync(id, dto) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            => await blogPostService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
