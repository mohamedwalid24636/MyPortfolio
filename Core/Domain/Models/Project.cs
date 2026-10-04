using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Project : BaseEntity<int>
{

    public string Title { get; set; }

    public string Description { get; set; }

    public string ShortDescription { get; set; }

    public string ImageUrl { get; set; }

    public string GithubUrl { get; set; }

    public string LiveDemoUrl { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; }

    public bool Featured { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }




    // Project <Has> Project_Tag 
    [InverseProperty(nameof(Project_Tag.Project))]
    public ICollection<Project_Tag>? Project_Tags { get; set; }




    // Project <Has> ProjectImage 
    [InverseProperty(nameof(ProjectImage.Project))]
    public ICollection<ProjectImage>? ProjectImages { get; set; }


    // Project <Has> Project_Category
    [InverseProperty(nameof(Project_Category.Project))]
    public ICollection<Project_Category>? Project_Categories { get; set; }


    // Project <Has> Project_Technology
    [InverseProperty(nameof(Project_Technology.Project))]
    public ICollection<Project_Technology>? Project_Technologies { get; set; }




}
