using E_commerce.Application.Specifications;
using E_commerce.Domain.Common;
using E_commerce.Domain.Contracts;
using E_commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, TKey>(StoreDbContext dbContext) : IGenericRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
    {
        public void Add(TEntity entity) => dbContext.Set<TEntity>().Add(entity);

        public void Remove(TEntity entity)
        => dbContext.Set<TEntity>().Add(entity);

        public void Update(TEntity entity)
       => dbContext.Set<TEntity>().Add(entity);
        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
        => await dbContext.Set<TEntity>().AsNoTracking().ToListAsync(ct);

        public async Task<TEntity?> GetByIdAsync(TKey Id, CancellationToken ct = default)
       => await dbContext.Set<TEntity>().FindAsync([Id],ct).AsTask();

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> Spec, CancellationToken ct = default)
        {
            var query=SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(),Spec);   
            return await query.ToListAsync(ct);



        }

        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> Spec, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), Spec);
            return await query.FirstOrDefaultAsync();
        }
    }
}
