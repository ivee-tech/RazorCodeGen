using Iv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Data
{
    public interface IDataCommandSpecification<T>
        where T : class
    {

        Action<T> Action { get; }
        void Prepare(T obj);

    }
}
