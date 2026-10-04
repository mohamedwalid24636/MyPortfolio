using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using ServiceAbstraction;
using Shared.DTOs.Common;

namespace Service.Services
{
    public abstract class GenericService<TEntity, TEntityDto, TCreateDto, TUpdateDto, TKey>
        : IGenericService<TEntityDto, TCreateDto, TUpdateDto, TKey>
        where TEntity : BaseEntity<TKey>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        protected GenericService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        protected IGenericRepository<TEntity, TKey> Repository => _unitOfWork.GetRepository<TEntity, TKey>();

        protected abstract ISpecifications<TEntity, TKey> CreateGetAllSpecification(QueryParameters parameters);
        protected abstract ISpecifications<TEntity, TKey> CreateCountSpecification(QueryParameters parameters);
        protected abstract ISpecifications<TEntity, TKey> CreateGetByIdSpecification(TKey id);

        public async Task<PaginationResult<TEntityDto>> GetAllAsync(QueryParameters parameters)
        {
            var entities = await Repository.GetAllAsync(CreateGetAllSpecification(parameters));
            var totalCount = await Repository.CountAsync(CreateCountSpecification(parameters));

            var data = _mapper.Map<IEnumerable<TEntityDto>>(entities);

            return new PaginationResult<TEntityDto>(totalCount, parameters.PageIndex, parameters.PageSize, data);
        }

        public async Task<TEntityDto?> GetByIdAsync(TKey id)
        {
            var entity = await Repository.GetByIdAsync(CreateGetByIdSpecification(id));

            return entity is null ? default : _mapper.Map<TEntityDto>(entity);
        }

        public virtual async Task<TEntityDto> CreateAsync(TCreateDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);

            await Repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TEntityDto>(entity);
        }

        public virtual async Task<bool> UpdateAsync(TKey id, TUpdateDto dto)
        {
            var entity = await Repository.GetByIdAsync(id);
            if (entity is null) return false;

            _mapper.Map(dto, entity);
            Repository.Update(entity);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> DeleteAsync(TKey id)
        {
            var entity = await Repository.GetByIdAsync(id);
            if (entity is null) return false;

            Repository.Delete(entity);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
