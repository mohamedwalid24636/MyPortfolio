using Domain.Models;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Specifications
{
    public class BlogPostSpecifications : BaseSpecification<BlogPost, int>
    {
        public BlogPostSpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(b => string.IsNullOrWhiteSpace(parameters.Search)
                     || b.Title.Contains(parameters.Search)
                     || b.ShortDescription.Contains(parameters.Search)
                     || b.Slug.Contains(parameters.Search))
        {
            AddOrderByDescending(b => b.PublishedAt!);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public BlogPostSpecifications(int id) : base(b => b.Id == id)
        {
        }

        // Matches on slug, optionally ignoring one row so a post can keep its own slug on update.
        public BlogPostSpecifications(string slug, int? excludingId)
            : base(b => b.Slug == slug && (excludingId == null || b.Id != excludingId))
        {
        }
    }
}
