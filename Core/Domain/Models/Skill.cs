using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Skill : BaseEntity<int>
{

    public string Name { get; set; }

    public string Description { get; set; }

    public string ProficiencyLevel { get; set; }

    public string IconUrl { get; set; }



    [InverseProperty(nameof(Skill_Type.Skill))]

    // Skill <Has> Skill_Type
   public ICollection<Skill_Type>? Skill_Types { get; set; } 

}
