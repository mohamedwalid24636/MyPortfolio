using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.Repositories
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery, ISpecifications<TEntity, TKey> specifications)
            where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;

            // Apply Where Condition
            if (specifications.Criteria is not null)
                query = query.Where(specifications.Criteria);

            // Apply Includes (supports ThenInclude chains for join entities)
            query = specifications.IncludeExpressions.Aggregate(query,
                (currentQuery, includeExpression) => includeExpression(currentQuery));

            // Apply Ordering
            if (specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);
            else if (specifications.OrderByDescending is not null)
                query = query.OrderByDescending(specifications.OrderByDescending);

            // Apply Pagination
            if (specifications.IsPagination)
                query = query.Skip(specifications.Skip).Take(specifications.Take);

            return query;
        }
    }
}
