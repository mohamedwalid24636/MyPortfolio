namespace Domain.Models;

public class Education : BaseEntity<int>
{

    public string InstitutionName { get; set; }

    public string Degree { get; set; }

    public string FieldOfStudy { get; set; }

    public string Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsCurrent { get; set; }

    public string InstitutionLogoUrl { get; set; }
}
