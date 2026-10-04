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
    public class SocialLinkService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAttachmentService attachmentService,
        IOptions<AttachmentOptions> options)
        : GenericService<SocialLink, SocialLinkDto, CreateSocialLinkDto, UpdateSocialLinkDto, int>(unitOfWork, mapper),
          ISocialLinkService
    {
        private readonly IAttachmentService _attachmentService = attachmentService;
        private readonly AttachmentOptions _attachmentOptions = options.Value;

        protected override ISpecifications<SocialLink, int> CreateGetAllSpecification(QueryParameters parameters)
            => new SocialLinkSpecifications(parameters);

        protected override ISpecifications<SocialLink, int> CreateCountSpecification(QueryParameters parameters)
            => new SocialLinkSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<SocialLink, int> CreateGetByIdSpecification(int id)
            => new SocialLinkSpecifications(id);

        public override async Task<SocialLinkDto> CreateAsync(CreateSocialLinkDto dto)
        {
            var socialLink = _mapper.Map<SocialLink>(dto);

            await _unitOfWork.CreateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
socialLink
,
                socialLink.IconUrl, dto.Icon, AttachmentFolderKeys.SocialLinkIcon, dto.IsDelete,
                (entity, resolution) => entity.IconUrl = resolution.StoredPath);

            return _mapper.Map<SocialLinkDto>(socialLink);
        }

        public override async Task<bool> UpdateAsync(int id, UpdateSocialLinkDto dto)
        {
            var socialLink = await Repository.GetByIdAsync(id);
            if (socialLink is null) return false;

            _mapper.Map(dto, socialLink);

            return await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
socialLink
,
                socialLink.IconUrl, dto.Icon, AttachmentFolderKeys.SocialLinkIcon, dto.IsDelete,
                (entity, resolution) => entity.IconUrl = resolution.StoredPath) > 0;
        }

        public override Task<bool> DeleteAsync(int id)
            => Repository.DeleteWithAttachmentsAsync(
                _unitOfWork, _attachmentService, id, socialLink => socialLink.IconUrl);
    }
}
