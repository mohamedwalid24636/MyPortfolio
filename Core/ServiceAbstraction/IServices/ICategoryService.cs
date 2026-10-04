using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface ICategoryService : IGenericService<CategoryDto, CreateCategoryDto, UpdateCategoryDto, int>
    {
    }
}
