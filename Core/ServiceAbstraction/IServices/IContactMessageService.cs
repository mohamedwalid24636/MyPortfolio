using Shared.DTOs;

namespace ServiceAbstraction
{
    public interface IContactMessageService : IGenericService<ContactMessageDto, CreateContactMessageDto, UpdateContactMessageDto, int>
    {
        Task<bool> MarkAsReadAsync(int id);
    }
}
