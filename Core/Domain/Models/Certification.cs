namespace Domain.Models;

public class Certification : BaseEntity<int>
{


    public string Name { get; set; }

    public string IssuingOrganization { get; set; }

    public string CredentialId { get; set; }

    public string CredentialUrl { get; set; }

    public DateTime? IssueDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool DoesNotExpire { get; set; }

    public string CertificateUrl { get; set; }
}
