using Iv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Iv.Data.GenericEF
{
    public class GenericDataQuery<T, TKey> : IDataQuery<T, TKey>
        where T : ObjectDefBase<TKey>
        where TKey : IComparable
    {

        protected IDataContext ctx;

        public GenericDataQuery(IDataContext ctx)
        {
            this.ctx = ctx;
        }

        public string StoreName => throw new NotImplementedException();

        public IEnumerable<T> Filter(IDataQuerySpecification<T> spec)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(ctx.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            return secondaryResult
                            .Where(spec.Criteria)
                            .AsEnumerable();
        }

        public IEnumerable<T> Filter(Expression<Func<T, bool>> predicate)
        {
            return ctx.Set<T>().Where(predicate);
        }

        public T Find(IDataQuerySpecification<T> spec)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(ctx.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            return secondaryResult
                            .FirstOrDefault(spec.Criteria);
        }

        public T Find(TKey key)
        {
            return ctx.Set<T>().Find(key);
        }

        public IEnumerable<T> GetAll()
        {
            return ctx.Set<T>();
        }

        public void Initialize(string storeName)
        {
            throw new NotImplementedException();
        }
    }
}
