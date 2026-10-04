using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.Extensions.Options;
using Service.Attachments;
using Service.Options;
using Service.Specifications;
using ServiceAbstraction;
using ServiceAbstraction.Attachments;
using Shared.DTOs;
using Shared.DTOs.Common;
using System.Globalization;
using System.Text;

namespace Service.Services
{
    public class BlogPostService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAttachmentService attachmentService,
        IOptions<AttachmentOptions> options)
        : GenericService<BlogPost, BlogPostDto, CreateBlogPostDto, UpdateBlogPostDto, int>(unitOfWork, mapper),
          IBlogPostService
    {
        private readonly IAttachmentService _attachmentService = attachmentService;
        private readonly AttachmentOptions _attachmentOptions = options.Value;

        private const string PublishedStatus = "Published";
        private const double WordsPerMinute = 200d;

        protected override ISpecifications<BlogPost, int> CreateGetAllSpecification(QueryParameters parameters)
            => new BlogPostSpecifications(parameters);

        protected override ISpecifications<BlogPost, int> CreateCountSpecification(QueryParameters parameters)
            => new BlogPostSpecifications(parameters, applyPagination: false);

        protected override ISpecifications<BlogPost, int> CreateGetByIdSpecification(int id)
            => new BlogPostSpecifications(id);

        public override async Task<BlogPostDto> CreateAsync(CreateBlogPostDto dto)
        {
            var blogPost = _mapper.Map<BlogPost>(dto);
            blogPost.Slug = await BuildUniqueSlugAsync(dto.Title, null);
            blogPost.ReadingTime = EstimateReadingTime(dto.Content);
            blogPost.UpdatedAt = DateTime.UtcNow;
            blogPost.PublishedAt = IsPublished(blogPost.Status) ? DateTime.UtcNow : null;

            await _unitOfWork.CreateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
blogPost
,
                blogPost.CoverImageUrl, dto.CoverImage, AttachmentFolderKeys.BlogPostCoverImage, dto.IsDelete,
                (entity, resolution) => entity.CoverImageUrl = resolution.StoredPath);

            return _mapper.Map<BlogPostDto>(blogPost);
        }

        public override async Task<bool> UpdateAsync(int id, UpdateBlogPostDto dto)
        {
            var blogPost = await Repository.GetByIdAsync(id);
            if (blogPost is null) return false;

            _mapper.Map(dto, blogPost);

            blogPost.Slug = await BuildUniqueSlugAsync(dto.Title, blogPost.Id);
            blogPost.ReadingTime = EstimateReadingTime(dto.Content);
            blogPost.UpdatedAt = DateTime.UtcNow;

            if (IsPublished(blogPost.Status) && blogPost.PublishedAt is null)
                blogPost.PublishedAt = DateTime.UtcNow;

            return await _unitOfWork.UpdateAttachmentAsync(
                Repository, _attachmentService, _attachmentOptions, 
blogPost
,
                blogPost.CoverImageUrl, dto.CoverImage, AttachmentFolderKeys.BlogPostCoverImage, dto.IsDelete,
                (entity, resolution) => entity.CoverImageUrl = resolution.StoredPath) > 0;
        }

        public override Task<bool> DeleteAsync(int id)
            => Repository.DeleteWithAttachmentsAsync(
                _unitOfWork, _attachmentService, id, blogPost => blogPost.CoverImageUrl);

        private static bool IsPublished(string? status)
            => string.Equals(status, PublishedStatus, StringComparison.OrdinalIgnoreCase);

        /// <summary>Derive a url-safe Slug from the title, appending a counter when it is taken.</summary>
        private async Task<string> BuildUniqueSlugAsync(string? title, int? excludingId)
        {
            var stem = Slugify(title);

            if (stem.Length == 0)
                stem = "post";

            var slug = stem;

            for (var suffix = 2; (await Repository.GetAllAsync(new BlogPostSpecifications(slug, excludingId))).Any(); suffix++)
                slug = $"{stem}-{suffix.ToString(CultureInfo.InvariantCulture)}";

            return slug;
        }

        private static string Slugify(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return string.Empty;

            // Decomposing lets us drop the combining marks left behind by transliterated letters.
            var decomposed = title.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder(decomposed.Length);

            foreach (var character in decomposed)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark)
                    continue;

                if (char.IsLetterOrDigit(character))
                {
                    builder.Append(character);
                }
                else if (builder.Length > 0 && builder[^1] != '-')
                {
                    builder.Append('-');
                }
            }

            return builder.ToString().Trim('-');
        }

        private static int EstimateReadingTime(string? content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return 0;

            var words = content.Split(
                [' ', '\t', '\r', '\n', ' '],
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length;

            return (int)Math.Ceiling(words / WordsPerMinute);
        }
    }
}
