using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DTOs;
using Shared.DTOs.Common;

namespace Service.Services
{
    public class ContactMessageService(IUnitOfWork unitOfWork, IMapper mapper)
        : GenericService<ContactMessage, ContactMessageDto, CreateContactMessageDto, UpdateContactMessageDto, int>(unitOfWork, mapper),
          IContactMessageService
    {
        protected override ISpecifications<ContactMessage, int> CreateGetAllSpecification(QueryParameters parameters)
            => new ContactMessageSpecifications(parameters);

        protected override ISpecifications<ContactMessage, int> CreateCountSpecification(QueryParameters parameters)
            => new ContactMessageSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<ContactMessage, int> CreateGetByIdSpecification(int id)
            => new ContactMessageSpecifications(id);

        public override async Task<ContactMessageDto> CreateAsync(CreateContactMessageDto dto)
        {
            var entity = _mapper.Map<ContactMessage>(dto);
            entity.SentAt = DateTime.UtcNow;
            entity.IsRead = false;

            await Repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ContactMessageDto>(entity);
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var entity = await Repository.GetByIdAsync(id);
            if (entity is null) return false;

            entity.IsRead = true;
            Repository.Update(entity);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
