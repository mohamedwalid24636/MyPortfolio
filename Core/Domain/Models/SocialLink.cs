namespace Domain.Models;

public class SocialLink : BaseEntity<int>
{

    public string Platform { get; set; }

    public string Username { get; set; }

    public string Url { get; set; }

    public string IconUrl { get; set; }
}
