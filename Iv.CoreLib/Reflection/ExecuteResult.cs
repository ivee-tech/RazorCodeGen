using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Reflection
{
    public class ExecuteResult
    {
        public ExecuteResult()
        {
            Errors = new List<string>();
        }

        public bool Success { get; set; }
        public ICollection<string> Errors { get; private set; }
        public object ObjectOutput { get; set; }
        public object MethodOutput { get; set; }

        public string ErrorString(string delim = "\r\n")
        {
            string output = Errors.Aggregate((result, item) => $"{result}{delim}{item}");
            return output;
        }
    }
}
