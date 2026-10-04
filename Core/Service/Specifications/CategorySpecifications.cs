using Domain.Models;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Specifications
{
    public class CategorySpecifications : BaseSpecification<Category, int>
    {
        public CategorySpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(c => string.IsNullOrWhiteSpace(parameters.Search)
                     || c.Name.Contains(parameters.Search)
                     || c.Slug.Contains(parameters.Search))
        {
            AddOrderBy(c => c.Name);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public CategorySpecifications(int id) : base(c => c.Id == id)
        {
        }
    }
}
