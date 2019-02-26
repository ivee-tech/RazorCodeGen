using Iv.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Data.GenericEF
{
    public class ModelBuilderBase<T, TKey> : IModelBuilder<T, TKey>
        where TKey : IComparable
        where T : ObjectDefBase<TKey>
    {
        public virtual void Ignore<T1, TKey1>(DbModelBuilder modelBuilder)
            where TKey1 : IComparable
            where T1 : ObjectDefBase<TKey1>
        {
            modelBuilder.Entity<T1>().Ignore(p => p.IsNew);
            modelBuilder.Entity<T1>().Ignore(p => p.IsDirty);
            modelBuilder.Entity<T1>().Ignore(p => p.IsDeleted);
            modelBuilder.Entity<T1>().Ignore(p => p.Key);
        }

        public virtual IQueryable<T> Include(IDbSet<T> set)
        {
            return set;
        }

        public virtual Func<T, bool> KeyEquals(TKey key)
        {
            return t => t.Key.Equals(key);
        }

        public virtual void Map(DbModelBuilder modelBuilder)
        {
        }

    }
}
