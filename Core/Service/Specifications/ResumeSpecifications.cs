using Domain.Models;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Specifications
{
    public class ResumeSpecifications : BaseSpecification<Resume, int>
    {
        public ResumeSpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(r => string.IsNullOrWhiteSpace(parameters.Search) || r.Title.Contains(parameters.Search))
        {
            AddOrderByDescending(r => r.UploadedAt);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public ResumeSpecifications(int id) : base(r => r.Id == id)
        {
        }

        public ResumeSpecifications(bool isActive) : base(r => r.IsActive == isActive)
        {
        }
    }
}
