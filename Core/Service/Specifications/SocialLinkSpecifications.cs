using Domain.Models;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Specifications
{
    public class SocialLinkSpecifications : BaseSpecification<SocialLink, int>
    {
        public SocialLinkSpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(s => string.IsNullOrWhiteSpace(parameters.Search)
                     || s.Platform.Contains(parameters.Search)
                     || s.Username.Contains(parameters.Search))
        {
            AddOrderBy(s => s.Platform);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public SocialLinkSpecifications(int id) : base(s => s.Id == id)
        {
        }
    }
}
