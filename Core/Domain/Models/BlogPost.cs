namespace Domain.Models;

public class BlogPost : BaseEntity<int>
{

    public string Title { get; set; }

    public string Slug { get; set; }

    public string ShortDescription { get; set; }

    public string Content { get; set; }

    public string CoverImageUrl { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string Status { get; set; }

    public int ReadingTime { get; set; }
}
