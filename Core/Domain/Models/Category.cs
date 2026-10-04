using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Category : BaseEntity<int>
{

    public string Name { get; set; }

    public string Description { get; set; }

    public string Slug { get; set; }


    [InverseProperty(nameof(Project_Category.Category))]
    // Category <Has> Project_Category
    public ICollection<Project_Category>? Project_Categories { get; set; }

}
