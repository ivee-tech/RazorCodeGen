using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Data
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class IgnoreColumnAttribute : Attribute
    {

        public DataOperation Operation { get; set; }

        public IgnoreColumnAttribute()
        {
            this.Operation = DataOperation.All;
        }

        public IgnoreColumnAttribute(DataOperation operation) : this()
        {
            this.Operation = operation;
        }
    }
}
