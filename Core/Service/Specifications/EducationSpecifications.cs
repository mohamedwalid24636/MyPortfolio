using Domain.Models;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Specifications
{
    public class EducationSpecifications : BaseSpecification<Education, int>
    {
        public EducationSpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(e => string.IsNullOrWhiteSpace(parameters.Search)
                     || e.InstitutionName.Contains(parameters.Search)
                     || e.Degree.Contains(parameters.Search))
        {
            AddOrderByDescending(e => e.StartDate!);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public EducationSpecifications(int id) : base(e => e.Id == id)
        {
        }
    }
}
