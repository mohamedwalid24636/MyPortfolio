using Domain.Models;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Specifications
{
    public class ProjectImageSpecifications : BaseSpecification<ProjectImage, int>
    {
        public ProjectImageSpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(p => string.IsNullOrWhiteSpace(parameters.Search) || p.Caption.Contains(parameters.Search))
        {
            AddOrderBy(p => p.DisplayOrder);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public ProjectImageSpecifications(int id) : base(p => p.Id == id)
        {
        }
    }
}
