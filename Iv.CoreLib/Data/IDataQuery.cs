using Iv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Data
{
    public interface IDataQuery<T, TKey> : IDataStore
        where T : ObjectDefBase<TKey>
        where TKey : IComparable
    {

        T Find(TKey key);
        T Find(IDataQuerySpecification<T> spec);
        IEnumerable<T> GetAll();
        IEnumerable<T> Filter(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Filter(IDataQuerySpecification<T> spec);

    }
}
