namespace Shared.DTOs;

public class ContactMessageDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}

public class CreateContactMessageDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
}

public class UpdateContactMessageDto
{
    public string Subject { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
}
