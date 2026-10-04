using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class SocialLinkDto
{
    public int Id { get; set; }
    public string Platform { get; set; }
    public string Username { get; set; }
    public string Url { get; set; }
    public string IconUrl { get; set; }
}

public class CreateSocialLinkDto
{
    public string Platform { get; set; }
    public string Username { get; set; }
    public string Url { get; set; }

    /// <summary>New platform icon to store.</summary>
    public IFormFile? Icon { get; set; }

    /// <summary>Remove the stored icon. When combined with <see cref="Icon"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}

public class UpdateSocialLinkDto
{
    public string Platform { get; set; }
    public string Username { get; set; }
    public string Url { get; set; }

    /// <summary>Replacement icon. Leave null to keep the stored one.</summary>
    public IFormFile? Icon { get; set; }

    /// <summary>Remove the stored icon. When combined with <see cref="Icon"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}