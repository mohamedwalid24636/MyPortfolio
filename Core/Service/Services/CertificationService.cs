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
    public class CertificationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAttachmentService attachmentService,
        IOptions<AttachmentOptions> options)
        : GenericService<Certification, CertificationDto, CreateCertificationDto, UpdateCertificationDto, int>(unitOfWork, mapper),
          ICertificationService
    {
        private readonly IAttachmentService _attachmentService = attachmentService;
        private readonly AttachmentOptions _attachmentOptions = options.Value;

        protected override ISpecifications<Certification, int> CreateGetAllSpecification(QueryParameters parameters)
            => new CertificationSpecifications(parameters);

        protected override ISpecifications<Certification, int> CreateCountSpecification(QueryParameters parameters)
            => new CertificationSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<Certification, int> CreateGetByIdSpecification(int id)
            => new CertificationSpecifications(id);

        public override async Task<CertificationDto> CreateAsync(CreateCertificationDto dto)
        {
            var certification = _mapper.Map<Certification>(dto);

            await _unitOfWork.CreateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
certification
,
                certification.CertificateUrl, dto.Certificate, AttachmentFolderKeys.CertificateFile, dto.IsDelete,
                (entity, resolution) => entity.CertificateUrl = resolution.StoredPath);

            return _mapper.Map<CertificationDto>(certification);
        }

        public override async Task<bool> UpdateAsync(int id, UpdateCertificationDto dto)
        {
            var certification = await Repository.GetByIdAsync(id);
            if (certification is null) return false;

            _mapper.Map(dto, certification);

            return await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
certification
,
                certification.CertificateUrl, dto.Certificate, AttachmentFolderKeys.CertificateFile, dto.IsDelete,
                (entity, resolution) => entity.CertificateUrl = resolution.StoredPath) > 0;
        }

        public override Task<bool> DeleteAsync(int id)
            => Repository.DeleteWithAttachmentsAsync(
                _unitOfWork, _attachmentService, id, certification => certification.CertificateUrl);
    }
}
