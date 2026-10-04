using Domain.Models;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Specifications
{
    public class ExperienceSpecifications : BaseSpecification<Experience, int>
    {
        public ExperienceSpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(e => string.IsNullOrWhiteSpace(parameters.Search)
                     || e.JobTitle.Contains(parameters.Search)
                     || e.CompanyName.Contains(parameters.Search))
        {
            AddOrderByDescending(e => e.StartDate!);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public ExperienceSpecifications(int id) : base(e => e.Id == id)
        {
        }
    }
}
