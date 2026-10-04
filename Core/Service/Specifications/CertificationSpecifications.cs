using Domain.Models;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Specifications
{
    public class CertificationSpecifications : BaseSpecification<Certification, int>
    {
        public CertificationSpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(c => string.IsNullOrWhiteSpace(parameters.Search)
                     || c.Name.Contains(parameters.Search)
                     || c.IssuingOrganization.Contains(parameters.Search))
        {
            AddOrderByDescending(c => c.IssueDate!);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public CertificationSpecifications(int id) : base(c => c.Id == id)
        {
        }
    }
}
