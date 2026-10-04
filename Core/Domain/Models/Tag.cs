using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Tag : BaseEntity<int>
{

    public string Name { get; set; }

    public string Slug { get; set; }


    [InverseProperty(nameof(Project_Tag.Tag))]
    // Tag <Has> Project_Tag 
    public ICollection<Project_Tag>? Project_Tags { get; set; }

}
