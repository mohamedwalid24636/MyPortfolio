using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.Extensions.Options;
using Service.Attachments;
using Service.Options;
using Service.Specifications;
using ServiceAbstraction;
using ServiceAbstraction.Attachments;
using Shared.DTOs;
using Shared.DTOs.Common;

namespace Service.Services
{
    public class TechnologyService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAttachmentService attachmentService,
        IOptions<AttachmentOptions> options)
        : GenericService<Technology, TechnologyDto, CreateTechnologyDto, UpdateTechnologyDto, int>(unitOfWork, mapper),
          ITechnologyService
    {
        private readonly IAttachmentService _attachmentService = attachmentService;
        private readonly AttachmentOptions _attachmentOptions = options.Value;

        protected override ISpecifications<Technology, int> CreateGetAllSpecification(QueryParameters parameters)
            => new TechnologySpecifications(parameters);

        protected override ISpecifications<Technology, int> CreateCountSpecification(QueryParameters parameters)
            => new TechnologySpecifications(parameters, applyPagination: false);

        protected override ISpecifications<Technology, int> CreateGetByIdSpecification(int id)
            => new TechnologySpecifications(id);

        public override async Task<TechnologyDto> CreateAsync(CreateTechnologyDto dto)
        {
            var technology = _mapper.Map<Technology>(dto);

            await _unitOfWork.CreateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
technology
,
                technology.IconUrl, dto.Icon, AttachmentFolderKeys.TechnologyIcon, dto.IsDelete,
                (entity, resolution) => entity.IconUrl = resolution.StoredPath);

            return _mapper.Map<TechnologyDto>(technology);
        }

        public override async Task<bool> UpdateAsync(int id, UpdateTechnologyDto dto)
        {
            var technology = await Repository.GetByIdAsync(id);
            if (technology is null) return false;

            _mapper.Map(dto, technology);

            return await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
technology
,
                technology.IconUrl, dto.Icon, AttachmentFolderKeys.TechnologyIcon, dto.IsDelete,
                (entity, resolution) => entity.IconUrl = resolution.StoredPath) > 0;
        }

        public override Task<bool> DeleteAsync(int id)
            => Repository.DeleteWithAttachmentsAsync(
                _unitOfWork, _attachmentService, id, technology => technology.IconUrl);
    }
}
