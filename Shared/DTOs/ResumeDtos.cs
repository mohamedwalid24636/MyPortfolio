using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class ResumeDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string FileUrl { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
    public DateTime UploadedAt { get; set; }
    public bool IsActive { get; set; }
}

public class CreateResumeDto
{
    public string Title { get; set; }
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// The document to store. FileName, FileType and FileUrl are all derived from the uploaded
    /// bytes on the server, so they are not part of this contract.
    /// </summary>
    public IFormFile? File { get; set; }

    /// <summary>Remove the stored document. When combined with <see cref="File"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}

public class UpdateResumeDto
{
    public string Title { get; set; }
    public bool IsActive { get; set; }

    /// <summary>Replacement document. Leave null to keep the stored one.</summary>
    public IFormFile? File { get; set; }

    /// <summary>Remove the stored document. When combined with <see cref="File"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}