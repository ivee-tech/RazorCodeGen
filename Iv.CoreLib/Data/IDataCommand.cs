using Iv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Data
{
    public interface IDataCommand<T, TKey> : IDataStore
        where T : ObjectDefBase<TKey>
        where TKey : IComparable
    {
        T Create(T entity);
        T Create(T entity, IDataCommandSpecification<T> spec);
        void Delete(T entity);
        T Update(T entity);
        T Update(T entity, IDataCommandSpecification<T> spec);

        void SetEntityState<TEntity, TEntityKey>(TEntity e)
            where TEntity : ObjectDefBase<TEntityKey>
            where TEntityKey : IComparable;

        void SetEntityState<TEntity, TEntityKey>(TEntity e, out TEntity output)
            where TEntity : ObjectDefBase<TEntityKey>
            where TEntityKey : IComparable;

    }
}
