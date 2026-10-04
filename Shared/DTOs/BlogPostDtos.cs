using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class BlogPostDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string ShortDescription { get; set; }
    public string Content { get; set; }
    public string CoverImageUrl { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Status { get; set; }
    public int ReadingTime { get; set; }
}

/// <remarks>
/// Slug, ReadingTime, PublishedAt, UpdatedAt and CoverImageUrl are all produced by the server,
/// so they are deliberately absent from the write contracts.
/// </remarks>
public class CreateBlogPostDto
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }

    /// <summary>New cover image to store.</summary>
    public IFormFile? CoverImage { get; set; }

    /// <summary>Remove the stored cover image. When combined with <see cref="CoverImage"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}

public class UpdateBlogPostDto
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }

    /// <summary>Replacement cover image. Leave null to keep the stored one.</summary>
    public IFormFile? CoverImage { get; set; }

    /// <summary>Remove the stored cover image. When combined with <see cref="CoverImage"/> the old file goes first.</summary>
    public bool IsDelete { get; set; }
}