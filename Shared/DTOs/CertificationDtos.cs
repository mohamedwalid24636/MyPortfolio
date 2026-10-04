using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class CertificationDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string IssuingOrganization { get; set; }
    public string CredentialId { get; set; }
    public string CredentialUrl { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool DoesNotExpire { get; set; }
    public string CertificateUrl { get; set; }
}

public class CreateCertificationDto
{
    public string Name { get; set; }
    public string IssuingOrganization { get; set; }
    public string CredentialId { get; set; }
    public string CredentialUrl { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool DoesNotExpire { get; set; }

    /// <summary>The certificate document to store (usually a PDF).</summary>
    public IFormFile? Certificate { get; set; }

    /// <summary>Remove the stored certificate. When combined with <see cref="Certificate"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}

public class UpdateCertificationDto
{
    public string Name { get; set; }
    public string IssuingOrganization { get; set; }
    public string CredentialId { get; set; }
    public string CredentialUrl { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool DoesNotExpire { get; set; }

    /// <summary>Replacement certificate. Leave null to keep the stored one.</summary>
    public IFormFile? Certificate { get; set; }

    /// <summary>Remove the stored certificate. When combined with <see cref="Certificate"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}