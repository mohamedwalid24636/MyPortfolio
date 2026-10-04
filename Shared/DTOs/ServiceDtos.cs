using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class ServiceDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int DisplayOrder { get; set; }
    public string IconUrl { get; set; }
    public bool IsActive { get; set; }
}

public class CreateServiceDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;

    /// <summary>New icon to store.</summary>
    public IFormFile? Icon { get; set; }

    /// <summary>Remove the stored icon. When combined with <see cref="Icon"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}

public class UpdateServiceDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    /// <summary>Replacement icon. Leave null to keep the stored one.</summary>
    public IFormFile? Icon { get; set; }

    /// <summary>Remove the stored icon. When combined with <see cref="Icon"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}