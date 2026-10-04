using Shared.DTOs.Common;

namespace ServiceAbstraction
{
    public interface IGenericService<TEntityDto, TCreateDto, TUpdateDto, TKey>
    {
        Task<PaginationResult<TEntityDto>> GetAllAsync(QueryParameters parameters);
        Task<TEntityDto?> GetByIdAsync(TKey id);
        Task<TEntityDto> CreateAsync(TCreateDto dto);
        Task<bool> UpdateAsync(TKey id, TUpdateDto dto);
        Task<bool> DeleteAsync(TKey id);
    }
}
