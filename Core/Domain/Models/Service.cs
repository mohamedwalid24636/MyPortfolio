namespace Domain.Models;

public class Service : BaseEntity<int>
{

    public string Title { get; set; }

    public string Description { get; set; }

    public int DisplayOrder { get; set; }

    public string IconUrl { get; set; }

    public bool IsActive { get; set; }
}
