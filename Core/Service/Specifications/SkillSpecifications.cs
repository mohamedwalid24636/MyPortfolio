using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Specifications
{
    public class SkillSpecifications : BaseSpecification<Skill, int>
    {
        public SkillSpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(s => string.IsNullOrWhiteSpace(parameters.Search)
                     || s.Name.Contains(parameters.Search)
                     || s.ProficiencyLevel.Contains(parameters.Search))
        {
            AddRelatedIncludes();
            AddOrderBy(s => s.Name);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public SkillSpecifications(int id) : base(s => s.Id == id)
        {
            AddRelatedIncludes();
        }

        // Skill_Type is an explicit join entity, so the Type side must be
        // included explicitly as well.
        private void AddRelatedIncludes()
            => AddInclude(query => query.Include(s => s.Skill_Types!).ThenInclude(st => st.Type!));
    }
}