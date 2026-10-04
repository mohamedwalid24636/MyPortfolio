using Domain.Models;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Specifications
{
    public class ProfileSpecifications : BaseSpecification<Profile, int>
    {
        public ProfileSpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(p => string.IsNullOrWhiteSpace(parameters.Search) || p.FullName.Contains(parameters.Search))
        {
            AddOrderBy(p => p.FullName);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public ProfileSpecifications(int id) : base(p => p.Id == id)
        {
        }
    }
}
