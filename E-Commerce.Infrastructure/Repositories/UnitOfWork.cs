using E_commerce.Domain.Common;
using E_commerce.Domain.Contracts;
using E_commerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Repositories
{
    public class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var TypeName=typeof(TEntity).Name;
            if(_repositories.TryGetValue(TypeName,out object? value))
                return (IGenericRepository<TEntity, TKey>)value;
            var Repo = new GenericRepository<TEntity, TKey>(dbContext);
            _repositories[TypeName] = Repo;
            return Repo;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => dbContext.SaveChangesAsync(ct);
    }
}
