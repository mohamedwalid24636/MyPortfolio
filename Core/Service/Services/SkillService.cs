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
    public class SkillService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAttachmentService attachmentService,
        IOptions<AttachmentOptions> options)
        : GenericService<Skill, SkillDto, CreateSkillDto, UpdateSkillDto, int>(unitOfWork, mapper),
          ISkillService
    {
        private readonly IAttachmentService _attachmentService = attachmentService;
        private readonly AttachmentOptions _attachmentOptions = options.Value;

        protected override ISpecifications<Skill, int> CreateGetAllSpecification(QueryParameters parameters)
            => new SkillSpecifications(parameters);

        protected override ISpecifications<Skill, int> CreateCountSpecification(QueryParameters parameters)
            => new SkillSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<Skill, int> CreateGetByIdSpecification(int id)
            => new SkillSpecifications(id);

        public override async Task<SkillDto> CreateAsync(CreateSkillDto dto)
        {
            var skill = _mapper.Map<Skill>(dto);

            // Replacing the types needs the generated id and its own flushes, so it shares the
            // icon's transaction rather than committing ahead of it.
            await _unitOfWork.CreateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, skill,
                skill.IconUrl, dto.Icon, AttachmentFolderKeys.SkillIcon, dto.IsDelete,
                (entity, resolution) => entity.IconUrl = resolution.StoredPath,
                () => SyncTypesAsync(skill.Id, dto.TypeIds));

            var created = await Repository.GetByIdAsync(new SkillSpecifications(skill.Id));

            return _mapper.Map<SkillDto>(created!);
        }

        public override async Task<bool> UpdateAsync(int id, UpdateSkillDto dto)
        {
            var skill = await Repository.GetByIdAsync(id);
            if (skill is null) return false;

            _mapper.Map(dto, skill);

            await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, skill,
                skill.IconUrl, dto.Icon, AttachmentFolderKeys.SkillIcon, dto.IsDelete,
                (entity, resolution) => entity.IconUrl = resolution.StoredPath,
                () => SyncTypesAsync(skill.Id, dto.TypeIds));

            return true;
        }

        public override Task<bool> DeleteAsync(int id)
            => Repository.DeleteWithAttachmentsAsync(
                _unitOfWork, _attachmentService, id, skill => skill.IconUrl);

        /// <summary>
        /// Replaces the skill's type assignments wholesale. The removals are flushed first so the
        /// insert below cannot collide with a row the same request is retiring.
        /// </summary>
        private async Task SyncTypesAsync(int skillId, List<int> typeIds)
        {
            var repository = _unitOfWork.Skill_Type_Table_Repository;

            var existing = await repository.GetAllAsync();

            foreach (var skillType in existing.Where(st => st.SkillId == skillId))
                repository.Delete(skillType);

            await _unitOfWork.SaveChangesAsync();

            foreach (var typeId in typeIds.Distinct())
                await repository.AddAsync(new Skill_Type { SkillId = skillId, TypeId = typeId });

            await _unitOfWork.SaveChangesAsync();
        }
    }
}