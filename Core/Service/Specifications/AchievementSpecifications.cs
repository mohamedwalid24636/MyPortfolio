using Domain.Models;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Specifications
{
    public class AchievementSpecifications : BaseSpecification<Achievement, int>
    {
        public AchievementSpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(a => string.IsNullOrWhiteSpace(parameters.Search) || a.Title.Contains(parameters.Search))
        {
            AddOrderByDescending(a => a.Date!);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public AchievementSpecifications(int id) : base(a => a.Id == id)
        {
        }
    }
}
