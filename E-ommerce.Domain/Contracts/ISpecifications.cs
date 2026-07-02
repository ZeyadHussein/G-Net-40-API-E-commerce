using E_commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Domain.Contracts
{
    public interface ISpecifications<TEntity,TKey>where TEntity:BaseEntity<TKey>
    {
        ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; }

        Expression<Func<TEntity,bool>>CrIteria { get; } 


    }
}
