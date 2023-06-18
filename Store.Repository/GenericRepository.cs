using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Repositories;
using Store.Core.Specifications;
using Store.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext storeDbContext;
        public GenericRepository(StoreDbContext storeDbContext)
        {
            this.storeDbContext = storeDbContext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
         => await storeDbContext.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
            => await  ApplySpecifications(spec).ToListAsync();

        public async Task<T> GetByIdAsync(int id)
            => await storeDbContext.Set<T>().FindAsync(id);

        public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> spec)
            => await ApplySpecifications(spec).FirstOrDefaultAsync();

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }
        public void Delete(T entity)
             => storeDbContext.Set<T>().Remove(entity);

        public async Task AddAsync(T entity)
             => await storeDbContext.Set<T>().AddAsync(entity);
        public void Update(T entity)
             => storeDbContext.Set<T>().Update(entity);
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await storeDbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }
        private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
            => SpecificationEvaluator<T>.GetQuery(storeDbContext.Set<T>(), spec);
       

    }
}
