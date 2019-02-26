using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Reflection
{
    public class CompileOptions
    {

        public CompileOptions()
        {
            // References = new List<Assembly>();
            ReferenceLocations = new List<string>();
        }

        public string AssemblyName { get; set; }
        public CompileOutputType OutputType { get; set; }
        // public ICollection<Assembly> References { get; private set;}
        public ICollection<string> ReferenceLocations { get; private set; }
        public string OutputDir { get; set; }
        public bool Recurse { get; set; }
    }
}
