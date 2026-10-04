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
    public class ProjectService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAttachmentService attachmentService,
        IOptions<AttachmentOptions> options)
        : GenericService<Project, ProjectDto, CreateProjectDto, UpdateProjectDto, int>(unitOfWork, mapper),
          IProjectService
    {
        private readonly IAttachmentService _attachmentService = attachmentService;
        private readonly AttachmentOptions _attachmentOptions = options.Value;

        protected override ISpecifications<Project, int> CreateGetAllSpecification(QueryParameters parameters)
            => new ProjectSpecifications(parameters);

        protected override ISpecifications<Project, int> CreateCountSpecification(QueryParameters parameters)
            => new ProjectSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<Project, int> CreateGetByIdSpecification(int id)
            => new ProjectSpecifications(id);

        public async Task<IEnumerable<ProjectDto>> GetFeaturedAsync()
        {
            var projects = await Repository.GetAllAsync(new ProjectSpecifications(true));

            return _mapper.Map<IEnumerable<ProjectDto>>(projects).ToList();
        }

        public override async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
        {
            var project = _mapper.Map<Project>(dto);
            project.CreatedAt = DateTime.UtcNow;
            project.UpdatedAt = DateTime.UtcNow;

            // Replacing the relations needs the generated id and its own flushes, so it shares the
            // attachment's transaction: the cover image and the relations commit together.
            await _unitOfWork.CreateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, project,
                project.ImageUrl, dto.Image, AttachmentFolderKeys.ProjectImage, dto.IsDelete,
                (entity, resolution) => entity.ImageUrl = resolution.StoredPath,
                () => SyncRelationsAsync(project.Id, dto.CategoryIds, dto.TagIds, dto.TechnologyIds));

            // Re-read so the response carries the categories, tags, technologies and gallery
            // images the create request supplied by id rather than by navigation.
            var created = await Repository.GetByIdAsync(new ProjectSpecifications(project.Id));

            return _mapper.Map<ProjectDto>(created!);
        }

        public override async Task<bool> UpdateAsync(int id, UpdateProjectDto dto)
        {
            var project = await Repository.GetByIdAsync(id);
            if (project is null) return false;

            _mapper.Map(dto, project);
            project.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, project,
                project.ImageUrl, dto.Image, AttachmentFolderKeys.ProjectImage, dto.IsDelete,
                (entity, resolution) => entity.ImageUrl = resolution.StoredPath,
                () => SyncRelationsAsync(project.Id, dto.CategoryIds, dto.TagIds, dto.TechnologyIds));

            return true;
        }

        /// <summary>
        /// Removes the project along with its cover image and every gallery image, so no file is
        /// left behind. The files are only unlinked once the row is actually gone.
        /// </summary>
        public override async Task<bool> DeleteAsync(int id)
        {
            var project = await Repository.GetByIdAsync(new ProjectSpecifications(id));
            if (project is null) return false;

            var coverPath = project.ImageUrl;
            var galleryPaths = project.ProjectImages?.Select(image => image.ImageUrl).ToList() ?? [];

            Repository.Delete(project);

            var deleted = await _unitOfWork.SaveChangesAsync() > 0;

            if (!deleted)
                return false;

            _attachmentService.Delete(coverPath);

            foreach (var galleryPath in galleryPaths)
                _attachmentService.Delete(galleryPath);

            return true;
        }

        /// <summary>
        /// Replaces the project's category, tag and technology assignments wholesale. The
        /// removals are flushed before the inserts so the same request cannot collide with a row it
        /// is retiring.
        /// </summary>
        private async Task SyncRelationsAsync(
            int projectId,
            List<int> categoryIds,
            List<int> tagIds,
            List<int> technologyIds)
        {
            var categoryRepository = _unitOfWork.Project_Category_Table_Repository;
            var tagRepository = _unitOfWork.Project_Tag_Table_Repository;
            var technologyRepository = _unitOfWork.Project_Technology_Table_Repository;

            foreach (var projectCategory in (await categoryRepository.GetAllAsync()).Where(pc => pc.ProjectId == projectId))
                categoryRepository.Delete(projectCategory);

            foreach (var projectTag in (await tagRepository.GetAllAsync()).Where(pt => pt.ProjectId == projectId))
                tagRepository.Delete(projectTag);

            foreach (var projectTechnology in (await technologyRepository.GetAllAsync()).Where(pt => pt.ProjectId == projectId))
                technologyRepository.Delete(projectTechnology);

            await _unitOfWork.SaveChangesAsync();

            foreach (var categoryId in categoryIds.Distinct())
                await categoryRepository.AddAsync(new Project_Category { ProjectId = projectId, CategoryId = categoryId });

            foreach (var tagId in tagIds.Distinct())
                await tagRepository.AddAsync(new Project_Tag { ProjectId = projectId, TagId = tagId });

            foreach (var technologyId in technologyIds.Distinct())
                await technologyRepository.AddAsync(new Project_Technology { ProjectId = projectId, TechnologyId = technologyId });

            await _unitOfWork.SaveChangesAsync();
        }
    }
}