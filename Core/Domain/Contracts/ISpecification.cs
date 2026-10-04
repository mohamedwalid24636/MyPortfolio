using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Contracts
{
    public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; }

        /// <summary>
        /// Include paths are stored as delegates rather than plain expressions so a
        /// specification can chain <c>ThenInclude</c>. This matters for explicit
        /// join entities (e.g. Skill -&gt; Skill_Type -&gt; Type): including only the
        /// join leaves the far side null, which then serialises as a null entry.
        /// </summary>
        public List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> IncludeExpressions { get; }

        public Expression<Func<TEntity, object>>? OrderBy { get; }
        public Expression<Func<TEntity, object>>? OrderByDescending { get; }
        public int Take { get; }
        public int Skip { get; }
        public bool IsPagination { get; }
    }
}
