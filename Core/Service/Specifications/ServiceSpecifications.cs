using Domain.Models;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceEntity = Domain.Models.Service;

namespace Service.Specifications
{
    public class ServiceSpecifications : BaseSpecification<ServiceEntity, int>
    {
        public ServiceSpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(s => string.IsNullOrWhiteSpace(parameters.Search) || s.Title.Contains(parameters.Search))
        {
            AddOrderBy(s => s.DisplayOrder);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public ServiceSpecifications(int id) : base(s => s.Id == id)
        {
        }
    }
}
