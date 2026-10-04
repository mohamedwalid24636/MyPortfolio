using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class EducationDto
{
    public int Id { get; set; }
    public string InstitutionName { get; set; }
    public string Degree { get; set; }
    public string FieldOfStudy { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public string InstitutionLogoUrl { get; set; }
}

public class CreateEducationDto
{
    public string InstitutionName { get; set; }
    public string Degree { get; set; }
    public string FieldOfStudy { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; }

    /// <summary>New institution logo to store.</summary>
    public IFormFile? InstitutionLogo { get; set; }

    /// <summary>Remove the stored logo. When combined with <see cref="InstitutionLogo"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}

public class UpdateEducationDto
{
    public string InstitutionName { get; set; }
    public string Degree { get; set; }
    public string FieldOfStudy { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; }

    /// <summary>Replacement logo. Leave null to keep the stored one.</summary>
    public IFormFile? InstitutionLogo { get; set; }

    /// <summary>Remove the stored logo. When combined with <see cref="InstitutionLogo"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}