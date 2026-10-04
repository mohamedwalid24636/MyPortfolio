namespace Domain.Models;

public class Profile : BaseEntity<int>
{

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
