using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RazorCodeGen
{
    public interface ICodeGenerator<I, O>
    {
        bool Process(I inputs);
        string[] Errors { get; set; }
        O Output { get; }
    }

}
