using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Iv.Common;
using Iv.Data;

namespace Iv.Reflection
{
    public static class AttributeHelper
    {
        public static bool HasIgnoreColumn(this PropertyInfo pi, DataOperation operation)
        {
            var attr = pi.GetCustomAttribute<IgnoreColumnAttribute>();
            if(attr == null)
            {
                return false;
            }
            return (attr.Operation & operation) == operation;
        }

    }
}
