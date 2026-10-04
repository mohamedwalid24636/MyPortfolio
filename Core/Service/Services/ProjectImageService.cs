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
    public class ProjectImageService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAttachmentService attachmentService,
        IOptions<AttachmentOptions> options)
        : GenericService<ProjectImage, ProjectImageDto, CreateProjectImageDto, UpdateProjectImageDto, int>(unitOfWork, mapper),
          IProjectImageService
    {
        private readonly IAttachmentService _attachmentService = attachmentService;
        private readonly AttachmentOptions _attachmentOptions = options.Value;

        protected override ISpecifications<ProjectImage, int> CreateGetAllSpecification(QueryParameters parameters)
            => new ProjectImageSpecifications(parameters);

        protected override ISpecifications<ProjectImage, int> CreateCountSpecification(QueryParameters parameters)
            => new ProjectImageSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<ProjectImage, int> CreateGetByIdSpecification(int id)
            => new ProjectImageSpecifications(id);

        public override async Task<ProjectImageDto> CreateAsync(CreateProjectImageDto dto)
        {
            var projectImage = _mapper.Map<ProjectImage>(dto);

            await _unitOfWork.CreateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
projectImage
,
                projectImage.ImageUrl, dto.Image, AttachmentFolderKeys.ProjectGalleryImage, dto.IsDelete,
                (entity, resolution) => entity.ImageUrl = resolution.StoredPath);

            return _mapper.Map<ProjectImageDto>(projectImage);
        }

        public override async Task<bool> UpdateAsync(int id, UpdateProjectImageDto dto)
        {
            var projectImage = await Repository.GetByIdAsync(id);
            if (projectImage is null) return false;

            _mapper.Map(dto, projectImage);

            return await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
projectImage
,
                projectImage.ImageUrl, dto.Image, AttachmentFolderKeys.ProjectGalleryImage, dto.IsDelete,
                (entity, resolution) => entity.ImageUrl = resolution.StoredPath) > 0;
        }

        public override Task<bool> DeleteAsync(int id)
            => Repository.DeleteWithAttachmentsAsync(
                _unitOfWork, _attachmentService, id, projectImage => projectImage.ImageUrl);
    }
}
