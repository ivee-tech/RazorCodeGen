using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iv.CodeGen
{
    public interface ICodeGenerator<I, O>
    {
        bool Process(I inputs);
        string[] Errors { get; set; }
        O Output { get; }
    }

}
