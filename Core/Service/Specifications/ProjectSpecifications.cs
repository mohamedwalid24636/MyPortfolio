using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Specifications
{
    public class ProjectSpecifications : BaseSpecification<Project, int>
    {
        public ProjectSpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(p => string.IsNullOrWhiteSpace(parameters.Search)
                     || p.Title.Contains(parameters.Search)
                     || p.ShortDescription.Contains(parameters.Search))
        {
            AddRelatedIncludes();
            AddOrderByDescending(p => p.CreatedAt);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public ProjectSpecifications(int id) : base(p => p.Id == id)
        {
            AddRelatedIncludes();
        }

        public ProjectSpecifications(bool featured) : base(p => p.Featured == featured)
        {
            AddRelatedIncludes();
            AddOrderByDescending(p => p.CreatedAt);
        }

        // ProjectImages is a plain dependent collection, but the three remaining
        // navigations are explicit join entities whose far sides must be
        // included too, otherwise they serialise as null entries.
        private void AddRelatedIncludes()
        {
            AddInclude(p => p.Include(x => x.ProjectImages!));
            AddInclude(p => p.Include(x => x.Project_Tags!).ThenInclude(x => x.Tag!));
            AddInclude(p => p.Include(x => x.Project_Categories!).ThenInclude(x => x.Category!));
            AddInclude(p => p.Include(x => x.Project_Technologies!).ThenInclude(x => x.Technology!));
        }
    }
}