using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DTOs;
using Shared.DTOs.Common;

namespace Service.Services
{
    public class TagService(IUnitOfWork unitOfWork, IMapper mapper)
        : GenericService<Tag, TagDto, CreateTagDto, UpdateTagDto, int>(unitOfWork, mapper),
          ITagService
    {
        protected override ISpecifications<Tag, int> CreateGetAllSpecification(QueryParameters parameters)
            => new TagSpecifications(parameters);

        protected override ISpecifications<Tag, int> CreateCountSpecification(QueryParameters parameters)
            => new TagSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<Tag, int> CreateGetByIdSpecification(int id)
            => new TagSpecifications(id);
    }
}
