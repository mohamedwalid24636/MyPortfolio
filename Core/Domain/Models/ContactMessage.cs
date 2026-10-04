namespace Domain.Models;

public class ContactMessage : BaseEntity<int>
{

    public string Name { get; set; }

    public string Email { get; set; }

    public string Subject { get; set; }

    public string Message { get; set; }

    public DateTime SentAt { get; set; }

    public bool IsRead { get; set; }
}
