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
    public class AchievementService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAttachmentService attachmentService,
        IOptions<AttachmentOptions> options)
        : GenericService<Achievement, AchievementDto, CreateAchievementDto, UpdateAchievementDto, int>(unitOfWork, mapper),
          IAchievementService
    {
        private readonly IAttachmentService _attachmentService = attachmentService;
        private readonly AttachmentOptions _attachmentOptions = options.Value;

        protected override ISpecifications<Achievement, int> CreateGetAllSpecification(QueryParameters parameters)
            => new AchievementSpecifications(parameters);

        protected override ISpecifications<Achievement, int> CreateCountSpecification(QueryParameters parameters)
            => new AchievementSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<Achievement, int> CreateGetByIdSpecification(int id)
            => new AchievementSpecifications(id);

        public override async Task<AchievementDto> CreateAsync(CreateAchievementDto dto)
        {
            var achievement = _mapper.Map<Achievement>(dto);

            await _unitOfWork.CreateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
achievement
,
                achievement.ImageUrl, dto.Image, AttachmentFolderKeys.AchievementImage, dto.IsDelete,
                (entity, resolution) => entity.ImageUrl = resolution.StoredPath);

            return _mapper.Map<AchievementDto>(achievement);
        }

        public override async Task<bool> UpdateAsync(int id, UpdateAchievementDto dto)
        {
            var achievement = await Repository.GetByIdAsync(id);
            if (achievement is null) return false;

            _mapper.Map(dto, achievement);

            return await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
achievement
,
                achievement.ImageUrl, dto.Image, AttachmentFolderKeys.AchievementImage, dto.IsDelete,
                (entity, resolution) => entity.ImageUrl = resolution.StoredPath) > 0;
        }

        public override Task<bool> DeleteAsync(int id)
            => Repository.DeleteWithAttachmentsAsync(
                _unitOfWork, _attachmentService, id, achievement => achievement.ImageUrl);
    }
}
