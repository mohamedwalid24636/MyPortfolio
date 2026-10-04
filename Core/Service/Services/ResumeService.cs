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
    public class ResumeService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAttachmentService attachmentService,
        IOptions<AttachmentOptions> options)
        : GenericService<Resume, ResumeDto, CreateResumeDto, UpdateResumeDto, int>(unitOfWork, mapper),
          IResumeService
    {
        private readonly IAttachmentService _attachmentService = attachmentService;
        private readonly AttachmentOptions _attachmentOptions = options.Value;

        protected override ISpecifications<Resume, int> CreateGetAllSpecification(QueryParameters parameters)
            => new ResumeSpecifications(parameters);

        protected override ISpecifications<Resume, int> CreateCountSpecification(QueryParameters parameters)
            => new ResumeSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<Resume, int> CreateGetByIdSpecification(int id)
            => new ResumeSpecifications(id);

        public override async Task<ResumeDto> CreateAsync(CreateResumeDto dto)
        {
            var resume = _mapper.Map<Resume>(dto);
            resume.UploadedAt = DateTime.UtcNow;

            // Standing down the previous active resume is passed as extra work rather than run
            // here, so its query and the attachment write share one transaction. Running it before
            // the commit meant a failing query stranded the upload; running it after the commit
            // meant a failing swap left the request half applied.
            await _unitOfWork.CreateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, resume,
                resume.FileUrl, dto.File, AttachmentFolderKeys.ResumeFile, dto.IsDelete,
                ApplyUpload,
                () => EnsureSingleActiveAsync(resume));

            return _mapper.Map<ResumeDto>(resume);
        }

        public override async Task<bool> UpdateAsync(int id, UpdateResumeDto dto)
        {
            var resume = await Repository.GetByIdAsync(id);
            if (resume is null) return false;

            _mapper.Map(dto, resume);

            await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, resume,
                resume.FileUrl, dto.File, AttachmentFolderKeys.ResumeFile, dto.IsDelete,
                ApplyUpload,
                () => EnsureSingleActiveAsync(resume));

            return true;
        }

        public override Task<bool> DeleteAsync(int id)
            => Repository.DeleteWithAttachmentsAsync(
                _unitOfWork, _attachmentService, id, resume => resume.FileUrl);

        public async Task<ResumeDto?> GetActiveAsync()
        {
            var resumes = await Repository.GetAllAsync(new ResumeSpecifications(true));

            var entity = resumes.FirstOrDefault();

            return entity is null ? null : _mapper.Map<ResumeDto>(entity);
        }

        /// <summary>
        /// FileUrl, FileName and FileType describe the bytes that were actually stored, so they are
        /// refreshed from the upload result rather than trusted from the request.
        /// </summary>
        private void ApplyUpload(Resume resume, AttachmentResolution resolution)
        {
            resume.FileUrl = resolution.StoredPath;

            if (!resolution.StoredNewFile)
                return;

            resume.FileName = resolution.FileName ?? string.Empty;
            resume.FileType = resolution.FileType ?? string.Empty;
            resume.UploadedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Only one resume may be flagged active, so any previous one is stood down. This runs as
        /// the attachment commit's extra work, which places it after the attachment's own save, so
        /// it has to flush the swap itself. Both saves share the one transaction, so the new
        /// attachment and the deactivation land together or not at all.
        /// </summary>
        private async Task EnsureSingleActiveAsync(Resume resume)
        {
            if (!resume.IsActive)
                return;

            var active = await Repository.GetAllAsync(new ResumeSpecifications(true));

            var superseded = active.Where(other => other.Id != resume.Id).ToList();

            if (superseded.Count == 0)
                return;

            foreach (var other in superseded)
            {
                other.IsActive = false;
                Repository.Update(other);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}