using Domain.Models;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Specifications
{
    public class ContactMessageSpecifications : BaseSpecification<ContactMessage, int>
    {
        public ContactMessageSpecifications(QueryParameters parameters, bool applyPagination = true)
            : base(m => string.IsNullOrWhiteSpace(parameters.Search)
                     || m.Name.Contains(parameters.Search)
                     || m.Email.Contains(parameters.Search)
                     || m.Subject.Contains(parameters.Search))
        {
            AddOrderByDescending(m => m.SentAt);

            if (applyPagination)
                ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }

        public ContactMessageSpecifications(int id) : base(m => m.Id == id)
        {
        }
    }
}
