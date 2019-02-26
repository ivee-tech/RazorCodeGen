using Iv.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Data.GenericEF
{
    public interface IModelBuilder<T, TKey>
        where TKey : IComparable
        where T : ObjectDefBase<TKey>
    {
        void Map(DbModelBuilder modelBuilder);
        void Ignore<T1, TKey1>(DbModelBuilder modelBuilder)
            where TKey1 : IComparable
            where T1 : ObjectDefBase<TKey1>;
    }
}
