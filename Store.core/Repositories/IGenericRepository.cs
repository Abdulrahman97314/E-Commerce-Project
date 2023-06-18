using Store.Core.Entities;
using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Repositories
{
    public interface IGenericRepository <T> where T : BaseEntity
    {
        public Task<IReadOnlyList<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
        public Task<T> GetEntityWithSpecAsync(ISpecifications<T> spec);
        public Task<int> GetCountWithSpecAsync(ISpecifications<T> spec);
        public void Delete(T entity);
        public Task AddAsync(T entity);
        public void Update(T entity);
        public Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
