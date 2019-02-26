using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Reflection
{
    public class CompileResult
    {
        public CompileResult()
        {
            Errors = new List<string>();
        }

        public bool Success { get; set; }
        public ICollection<string> Errors { get; private set; }
        public Assembly Assembly { get; set; }

        public string ErrorString(string delim = "\r\n")
        {
            string output = Errors.Aggregate((result, item) => $"{result}{delim}{item}");
            return output;
        }
    }
}
