using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Service.Specifications
{
    public abstract class BaseSpecification<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected BaseSpecification(Expression<Func<TEntity, bool>>? CriteriaExpression)
        {
            Criteria = CriteriaExpression;
        }

        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }


        #region Include
        public List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> IncludeExpressions { get; } = [];

        protected void AddInclude(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression)
        {
            IncludeExpressions.Add(includeExpression);
        }
        #endregion


        #region OrderBy
        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExp) => OrderBy = orderByExp;
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExp) => OrderByDescending = orderByDescExp;
        #endregion


        #region Pagination
        //TotalCount = 40 
        //PageSize = 10
        //Page Index = 3
        //10,10,10,10       //Skip = (PageIndex - 1) * PageSize = (3-1) * 10 = 20

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagination { get; set; }   // Default = false , Public set => To Set Value Freedomelly 
        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            IsPagination = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }
        #endregion





    }
}
