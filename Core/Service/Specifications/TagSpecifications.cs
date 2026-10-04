using Domain.Models;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Specifications
{
    public class TagSpecifications : BaseSpecification<Tag, int>
    {
        public TagSpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(t => string.IsNullOrWhiteSpace(parameters.Search)
                     || t.Name.Contains(parameters.Search)
                     || t.Slug.Contains(parameters.Search))
        {
            AddOrderBy(t => t.Name);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public TagSpecifications(int id) : base(t => t.Id == id)
        {
        }
    }
}
