using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Reflection
{
    public class ExecuteOptions
    {

        public ExecuteOptions()
        {
            ExecuteParameters = new List<object>();
        }

        public string DefaultNamespace { get; set; }
        public string DefaultClassName { get; set; }
        public string Method { get; set; }
        public ICollection<object> ExecuteParameters { get; private set; }
    }
}
