using Iv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Data.GenericEF
{
    public abstract class DataCommandSpecificationBase<T> : IDataCommandSpecification<T>
        where T : class
    {

        public DataCommandSpecificationBase(Action<T> action)
        {
            Action = action;
        }

        public Action<T> Action { get; } = null;

        public void Prepare(T obj)
        {
            if(Action != null)
            {
                Action(obj);
            }
        }
    }
}
