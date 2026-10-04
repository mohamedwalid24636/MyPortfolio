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
    public class EducationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAttachmentService attachmentService,
        IOptions<AttachmentOptions> options)
        : GenericService<Education, EducationDto, CreateEducationDto, UpdateEducationDto, int>(unitOfWork, mapper),
          IEducationService
    {
        private readonly IAttachmentService _attachmentService = attachmentService;
        private readonly AttachmentOptions _attachmentOptions = options.Value;

        protected override ISpecifications<Education, int> CreateGetAllSpecification(QueryParameters parameters)
            => new EducationSpecifications(parameters);

        protected override ISpecifications<Education, int> CreateCountSpecification(QueryParameters parameters)
            => new EducationSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<Education, int> CreateGetByIdSpecification(int id)
            => new EducationSpecifications(id);

        public override async Task<EducationDto> CreateAsync(CreateEducationDto dto)
        {
            var education = _mapper.Map<Education>(dto);

            await _unitOfWork.CreateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
education
,
                education.InstitutionLogoUrl, dto.InstitutionLogo, AttachmentFolderKeys.EducationLogo, dto.IsDelete,
                (entity, resolution) => entity.InstitutionLogoUrl = resolution.StoredPath);

            return _mapper.Map<EducationDto>(education);
        }

        public override async Task<bool> UpdateAsync(int id, UpdateEducationDto dto)
        {
            var education = await Repository.GetByIdAsync(id);
            if (education is null) return false;

            _mapper.Map(dto, education);

            return await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
education
,
                education.InstitutionLogoUrl, dto.InstitutionLogo, AttachmentFolderKeys.EducationLogo, dto.IsDelete,
                (entity, resolution) => entity.InstitutionLogoUrl = resolution.StoredPath) > 0;
        }

        public override Task<bool> DeleteAsync(int id)
            => Repository.DeleteWithAttachmentsAsync(
                _unitOfWork, _attachmentService, id, education => education.InstitutionLogoUrl);
    }
}
