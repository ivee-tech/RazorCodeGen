using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.CodeGen
{
    public class CodeGenArgs
    {
        public string InputFileName { get; set; }
        public string OutputFileName { get; set; }
        public string FileExtension { get; set; }
        public List<TemplateParameter> Parameters { get; private set; }

        public CodeGenArgs()
        {
            Parameters = new List<TemplateParameter>();
        }
    }
}
