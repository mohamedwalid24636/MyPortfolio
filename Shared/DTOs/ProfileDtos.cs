using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class ProfileDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string ProfessionalTitle { get; set; }
    public string Bio { get; set; }
    public string ProfileImageUrl { get; set; }
    public string AboutImageUrl { get; set; }
    public string Location { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int YearsOfExperience { get; set; }
}

public class CreateProfileDto
{
    public string FullName { get; set; }
    public string ProfessionalTitle { get; set; }
    public string Bio { get; set; }
    public string Location { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int YearsOfExperience { get; set; }

    /// <summary>New picture to store. The server decides the folder, name and URL.</summary>
    public IFormFile? Image { get; set; }

    public IFormFile? AboutImage { get; set; }

    /// <summary>Remove the stored picture. When combined with <see cref="Image"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
    public bool IsDeleteAboutImage { get; set; }
}

public class UpdateProfileDto
{
    public string FullName { get; set; }
    public string ProfessionalTitle { get; set; }
    public string Bio { get; set; }
    public string Location { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int YearsOfExperience { get; set; }

    /// <summary>Replacement picture. Leave null to keep the stored one.</summary>
    public IFormFile? Image { get; set; }

    public IFormFile? AboutImage { get; set; }

    /// <summary>Remove the stored picture. When combined with <see cref="Image"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
    public bool IsDeleteAboutImage { get; set; }
}