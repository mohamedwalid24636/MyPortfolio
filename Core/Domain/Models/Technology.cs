using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Technology : BaseEntity<int>
{

    public string Name { get; set; }

    public string Description { get; set; }

    public string IconUrl { get; set; }

    public string Category { get; set; }


    [InverseProperty(nameof(Project_Technology.Technology))]
    // Technology <Has> Project_Technology
    public ICollection<Project_Technology>? Project_Technologies { get; set; }

}
