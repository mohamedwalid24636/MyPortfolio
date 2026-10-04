using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class ProjectImageDto
{
    public int Id { get; set; }
    public string Caption { get; set; }
    public string ImageUrl { get; set; }
    public string DisplayOrder { get; set; }
    public int ProjectId { get; set; }
}

public class CreateProjectImageDto
{
    public string Caption { get; set; }
    public string DisplayOrder { get; set; }
    public int ProjectId { get; set; }

    /// <summary>New gallery image to store.</summary>
    public IFormFile? Image { get; set; }

    /// <summary>Remove the stored image. When combined with <see cref="Image"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}

public class UpdateProjectImageDto
{
    public string Caption { get; set; }
    public string DisplayOrder { get; set; }
    public int ProjectId { get; set; }

    /// <summary>Replacement image. Leave null to keep the stored one.</summary>
    public IFormFile? Image { get; set; }

    /// <summary>Remove the stored image. When combined with <see cref="Image"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}