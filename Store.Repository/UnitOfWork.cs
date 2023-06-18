using Store.Core;
using Store.Core.Entities;
using Store.Core.Repositories;
using Store.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext storeDbContext;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UnitOfWork(StoreDbContext storeDbContext)
        {
            this.storeDbContext = storeDbContext;
        }

        public async Task<int> CompleteAsync()
        {
            return await storeDbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await storeDbContext.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity);

            if (!repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(storeDbContext);
                repositories.Add(type, repository);
            }

            return (IGenericRepository<TEntity>)repositories[type];
        }
    }
}