using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DTOs;
using Shared.DTOs.Common;

namespace Service.Services
{
    public class CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        : GenericService<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto, int>(unitOfWork, mapper),
          ICategoryService
    {
        protected override ISpecifications<Category, int> CreateGetAllSpecification(QueryParameters parameters)
            => new CategorySpecifications(parameters);

        protected override ISpecifications<Category, int> CreateCountSpecification(QueryParameters parameters)
            => new CategorySpecifications(parameters, applyPagination: false);

        protected override ISpecifications<Category, int> CreateGetByIdSpecification(int id)
            => new CategorySpecifications(id);
    }
}
