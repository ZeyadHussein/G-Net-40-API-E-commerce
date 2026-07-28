using E_commerce.Domain.Common;
using E_commerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Specifications
{
    public class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

        // Implement the interface property with the correct name and signature
        public Expression<Func<TEntity, bool>> CrIteria => Criteria;

        public Expression<Func<TEntity, bool>> Criteria { get; private set; }

        public Expression<Func<TEntity, object>> OrderBy { get;private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; private set; }
        protected void ApplyPagination(int pageSize, int PageIndex)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = (PageIndex - 1) * pageSize;
        }

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }   

        protected BaseSpecifications(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }
        protected void AddInclude(Expression<Func<TEntity, object>> include)
        {
            IncludeExpressions.Add(include);
        }
    }
}
