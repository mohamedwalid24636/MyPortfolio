namespace Domain.Models;

public class Achievement : BaseEntity<int>
{

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime? Date { get; set; }

    public string ImageUrl { get; set; }

    public string Url { get; set; }
}
