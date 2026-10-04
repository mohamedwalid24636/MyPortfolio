using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class ProjectDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public string ImageUrl { get; set; }
    public string GithubUrl { get; set; }
    public string LiveDemoUrl { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; }
    public bool Featured { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<ProjectImageDto> Images { get; set; } = [];
    public List<TagDto> Tags { get; set; } = [];
    public List<CategoryDto> Categories { get; set; } = [];
    public List<TechnologyDto> Technologies { get; set; } = [];
}

public class CreateProjectDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public string GithubUrl { get; set; }
    public string LiveDemoUrl { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; }
    public bool Featured { get; set; }

    public List<int> CategoryIds { get; set; } = [];
    public List<int> TagIds { get; set; } = [];
    public List<int> TechnologyIds { get; set; } = [];

    /// <summary>New cover image to store.</summary>
    public IFormFile? Image { get; set; }

    /// <summary>Remove the stored cover image. When combined with <see cref="Image"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}

public class UpdateProjectDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public string GithubUrl { get; set; }
    public string LiveDemoUrl { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; }
    public bool Featured { get; set; }

    public List<int> CategoryIds { get; set; } = [];
    public List<int> TagIds { get; set; } = [];
    public List<int> TechnologyIds { get; set; } = [];

    /// <summary>Replacement cover image. Leave null to keep the stored one.</summary>
    public IFormFile? Image { get; set; }

    /// <summary>Remove the stored cover image. When combined with <see cref="Image"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}