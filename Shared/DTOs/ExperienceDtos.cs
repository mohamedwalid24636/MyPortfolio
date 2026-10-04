using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class ExperienceDto
{
    public int Id { get; set; }
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public string CompanyLogoUrl { get; set; }
    public string Location { get; set; }
    public string EmploymentType { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; }
}

public class CreateExperienceDto
{
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public string Location { get; set; }
    public string EmploymentType { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; }

    /// <summary>New company logo to store.</summary>
    public IFormFile? CompanyLogo { get; set; }

    /// <summary>Remove the stored logo. When combined with <see cref="CompanyLogo"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}

public class UpdateExperienceDto
{
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public string Location { get; set; }
    public string EmploymentType { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; }

    /// <summary>Replacement logo. Leave null to keep the stored one.</summary>
    public IFormFile? CompanyLogo { get; set; }

    /// <summary>Remove the stored logo. When combined with <see cref="CompanyLogo"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}