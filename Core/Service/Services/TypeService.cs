using AutoMapper;
using Domain.Contracts;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DTOs;
using Shared.DTOs.Common;
using TypeEntity = Domain.Models.Type;

namespace Service.Services
{
    public class TypeService(IUnitOfWork unitOfWork, IMapper mapper)
        : GenericService<TypeEntity, TypeDto, CreateTypeDto, UpdateTypeDto, int>(unitOfWork, mapper),
          ITypeService
    {
        protected override ISpecifications<TypeEntity, int> CreateGetAllSpecification(QueryParameters parameters)
            => new TypeSpecifications(parameters);

        protected override ISpecifications<TypeEntity, int> CreateCountSpecification(QueryParameters parameters)
            => new TypeSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<TypeEntity, int> CreateGetByIdSpecification(int id)
            => new TypeSpecifications(id);
    }
}
