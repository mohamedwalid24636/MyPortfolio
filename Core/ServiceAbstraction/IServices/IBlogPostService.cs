using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface IBlogPostService : IGenericService<BlogPostDto, CreateBlogPostDto, UpdateBlogPostDto, int>
    {
    }
}
