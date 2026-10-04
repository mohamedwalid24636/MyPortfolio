namespace Domain.Models;

public class Resume : BaseEntity<int>
{

    public string Title { get; set; }

    public string FileUrl { get; set; }

    public string FileName { get; set; }

    public string FileType { get; set; }

    public DateTime UploadedAt { get; set; }

    public bool IsActive { get; set; }
}
