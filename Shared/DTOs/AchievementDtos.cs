using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class AchievementDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? Date { get; set; }
    public string ImageUrl { get; set; }
    public string Url { get; set; }
}

public class CreateAchievementDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? Date { get; set; }
    public string Url { get; set; }

    /// <summary>New badge/certificate image to store.</summary>
    public IFormFile? Image { get; set; }

    /// <summary>Remove the stored image. When combined with <see cref="Image"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}

public class UpdateAchievementDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? Date { get; set; }
    public string Url { get; set; }

    /// <summary>Replacement image. Leave null to keep the stored one.</summary>
    public IFormFile? Image { get; set; }

    /// <summary>Remove the stored image. When combined with <see cref="Image"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}