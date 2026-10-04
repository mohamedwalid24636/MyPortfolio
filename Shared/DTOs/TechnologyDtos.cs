using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class TechnologyDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconUrl { get; set; }
    public string Category { get; set; }
}

public class CreateTechnologyDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }

    /// <summary>New technology icon to store.</summary>
    public IFormFile? Icon { get; set; }

    /// <summary>Remove the stored icon. When combined with <see cref="Icon"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}

public class UpdateTechnologyDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }

    /// <summary>Replacement icon. Leave null to keep the stored one.</summary>
    public IFormFile? Icon { get; set; }

    /// <summary>Remove the stored icon. When combined with <see cref="Icon"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}