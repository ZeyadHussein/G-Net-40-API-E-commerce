using E_commerce.Domain.Common;
using E_commerce.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Specifications
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity>CreateQuery<TEntity,TKey>(IQueryable<TEntity>inputQuery,ISpecifications<TEntity,TKey>Spec)where TEntity:BaseEntity<TKey>
        {
            var query = inputQuery;
            if(Spec.CrIteria != null)
            {
                query = query.Where(Spec.CrIteria);
            }
            if (Spec.OrderBy != null)
            {
                query = query.OrderBy(Spec.OrderBy);
            }
            if (Spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(Spec.OrderByDescending);
            }
            if (Spec.IncludeExpressions.Any())
            {
                query=Spec.IncludeExpressions.Aggregate(query,(current,NextExp)=>current.Include(NextExp));

            }
            if(Spec.IsPaginated)
            {
                query = query.Skip(Spec.Skip).Take(Spec.Take);
            }


            return query;
                
        }
    }
}
