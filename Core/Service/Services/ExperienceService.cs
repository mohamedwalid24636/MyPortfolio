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
    public class ExperienceService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAttachmentService attachmentService,
        IOptions<AttachmentOptions> options)
        : GenericService<Experience, ExperienceDto, CreateExperienceDto, UpdateExperienceDto, int>(unitOfWork, mapper),
          IExperienceService
    {
        private readonly IAttachmentService _attachmentService = attachmentService;
        private readonly AttachmentOptions _attachmentOptions = options.Value;

        protected override ISpecifications<Experience, int> CreateGetAllSpecification(QueryParameters parameters)
            => new ExperienceSpecifications(parameters);

        protected override ISpecifications<Experience, int> CreateCountSpecification(QueryParameters parameters)
            => new ExperienceSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<Experience, int> CreateGetByIdSpecification(int id)
            => new ExperienceSpecifications(id);

        public override async Task<ExperienceDto> CreateAsync(CreateExperienceDto dto)
        {
            var experience = _mapper.Map<Experience>(dto);

            await _unitOfWork.CreateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
experience
,
                experience.CompanyLogoUrl, dto.CompanyLogo, AttachmentFolderKeys.ExperienceLogo, dto.IsDelete,
                (entity, resolution) => entity.CompanyLogoUrl = resolution.StoredPath);

            return _mapper.Map<ExperienceDto>(experience);
        }

        public override async Task<bool> UpdateAsync(int id, UpdateExperienceDto dto)
        {
            var experience = await Repository.GetByIdAsync(id);
            if (experience is null) return false;

            _mapper.Map(dto, experience);

            return await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
experience
,
                experience.CompanyLogoUrl, dto.CompanyLogo, AttachmentFolderKeys.ExperienceLogo, dto.IsDelete,
                (entity, resolution) => entity.CompanyLogoUrl = resolution.StoredPath) > 0;
        }

        public override Task<bool> DeleteAsync(int id)
            => Repository.DeleteWithAttachmentsAsync(
                _unitOfWork, _attachmentService, id, experience => experience.CompanyLogoUrl);
    }
}
