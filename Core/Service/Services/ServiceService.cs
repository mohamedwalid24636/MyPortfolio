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
using ServiceEntity = Domain.Models.Service;

namespace Service.Services
{
    public class ServiceService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAttachmentService attachmentService,
        IOptions<AttachmentOptions> options)
        : GenericService<ServiceEntity, ServiceDto, CreateServiceDto, UpdateServiceDto, int>(unitOfWork, mapper),
          IServiceService
    {
        private readonly IAttachmentService _attachmentService = attachmentService;
        private readonly AttachmentOptions _attachmentOptions = options.Value;

        protected override ISpecifications<ServiceEntity, int> CreateGetAllSpecification(QueryParameters parameters)
            => new ServiceSpecifications(parameters);

        protected override ISpecifications<ServiceEntity, int> CreateCountSpecification(QueryParameters parameters)
            => new ServiceSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<ServiceEntity, int> CreateGetByIdSpecification(int id)
            => new ServiceSpecifications(id);

        public override async Task<ServiceDto> CreateAsync(CreateServiceDto dto)
        {
            var service = _mapper.Map<ServiceEntity>(dto);

            await _unitOfWork.CreateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
service
,
                service.IconUrl, dto.Icon, AttachmentFolderKeys.ServiceIcon, dto.IsDelete,
                (entity, resolution) => entity.IconUrl = resolution.StoredPath);

            return _mapper.Map<ServiceDto>(service);
        }

        public override async Task<bool> UpdateAsync(int id, UpdateServiceDto dto)
        {
            var service = await Repository.GetByIdAsync(id);
            if (service is null) return false;

            _mapper.Map(dto, service);

            return await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
service
,
                service.IconUrl, dto.Icon, AttachmentFolderKeys.ServiceIcon, dto.IsDelete,
                (entity, resolution) => entity.IconUrl = resolution.StoredPath) > 0;
        }

        public override Task<bool> DeleteAsync(int id)
            => Repository.DeleteWithAttachmentsAsync(
                _unitOfWork, _attachmentService, id, service => service.IconUrl);
    }
}
