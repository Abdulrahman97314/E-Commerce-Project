using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        Expression<Func<T, bool>> Criteria { get;}
        List<Expression<Func<T,object>>> Includes { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        public int Skip { get; }
        public int Take { get; }
        public bool IsPaginationEnabled { get; }
    }
}
