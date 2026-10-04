using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class SkillDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ProficiencyLevel { get; set; }
    public string IconUrl { get; set; }

    public List<TypeDto> Types { get; set; } = [];
}

public class CreateSkillDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ProficiencyLevel { get; set; }

    public List<int> TypeIds { get; set; } = [];

    /// <summary>New skill icon to store.</summary>
    public IFormFile? Icon { get; set; }

    /// <summary>Remove the stored icon. When combined with <see cref="Icon"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}

public class UpdateSkillDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ProficiencyLevel { get; set; }

    public List<int> TypeIds { get; set; } = [];

    /// <summary>Replacement icon. Leave null to keep the stored one.</summary>
    public IFormFile? Icon { get; set; }

    /// <summary>Remove the stored icon. When combined with <see cref="Icon"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}