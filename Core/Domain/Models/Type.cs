using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Type : BaseEntity<int>
{

    public string Name { get; set; }

    public string Description { get; set; }

    [InverseProperty(nameof(Skill_Type.Type))]
    // Type <Has> Skill_Type
    public ICollection<Skill_Type>? Skill_Types { get; set; }
}
