using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorCodeGen
{
    public class RazorCodeGenInput
    {
        public string Template { get; set; }
        public object Model { get; set; }
    }
}
