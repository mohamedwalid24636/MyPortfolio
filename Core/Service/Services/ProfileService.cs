using AutoMapper;
using Domain.Contracts;
using Microsoft.Extensions.Options;
using Service.Attachments;
using Service.Options;
using Service.Specifications;
using ServiceAbstraction;
using ServiceAbstraction.Attachments;
using Shared.DTOs;
using Shared.DTOs.Common;
using ProfileEntity = Domain.Models.Profile;

namespace Service.Services
{
    public class ProfileService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAttachmentService attachmentService,
        IOptions<AttachmentOptions> options)
        : GenericService<ProfileEntity, ProfileDto, CreateProfileDto, UpdateProfileDto, int>(unitOfWork, mapper),
          IProfileService
    {
        private readonly IAttachmentService _attachmentService = attachmentService;
        private readonly AttachmentOptions _attachmentOptions = options.Value;

        protected override ISpecifications<ProfileEntity, int> CreateGetAllSpecification(QueryParameters parameters)
            => new ProfileSpecifications(parameters);

        protected override ISpecifications<ProfileEntity, int> CreateCountSpecification(QueryParameters parameters)
            => new ProfileSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<ProfileEntity, int> CreateGetByIdSpecification(int id)
            => new ProfileSpecifications(id);

        public override async Task<ProfileDto> CreateAsync(CreateProfileDto dto)
        {
            var profile = _mapper.Map<ProfileEntity>(dto);

            await _unitOfWork.CreateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, profile,
                profile.ProfileImageUrl, dto.Image, AttachmentFolderKeys.ProfileImage, dto.IsDelete,
                (entity, resolution) => entity.ProfileImageUrl = resolution.StoredPath);

            await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, profile,
                profile.AboutImageUrl, dto.AboutImage, AttachmentFolderKeys.AboutImage, dto.IsDeleteAboutImage,
                (entity, resolution) => entity.AboutImageUrl = resolution.StoredPath);

            return _mapper.Map<ProfileDto>(profile);
        }

        public override async Task<bool> UpdateAsync(int id, UpdateProfileDto dto)
        {
            var profile = await Repository.GetByIdAsync(id);
            if (profile is null) return false;

            _mapper.Map(dto, profile);

            var updated = await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, profile,
                profile.ProfileImageUrl, dto.Image, AttachmentFolderKeys.ProfileImage, dto.IsDelete,
                (entity, resolution) => entity.ProfileImageUrl = resolution.StoredPath);

            var aboutUpdated = await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, profile,
                profile.AboutImageUrl, dto.AboutImage, AttachmentFolderKeys.AboutImage, dto.IsDeleteAboutImage,
                (entity, resolution) => entity.AboutImageUrl = resolution.StoredPath);

            return updated > 0 || aboutUpdated > 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            var profile = await Repository.GetByIdAsync(id);
            if (profile is null) return false;

            Repository.Delete(profile);
            var deleted = await _unitOfWork.SaveChangesAsync() > 0;
            if (!deleted) return false;

            _attachmentService.Delete(profile.ProfileImageUrl);
            _attachmentService.Delete(profile.AboutImageUrl);
            return true;
        }
    }
}