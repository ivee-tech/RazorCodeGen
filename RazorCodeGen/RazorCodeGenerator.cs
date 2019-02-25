using RazorTemplates.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorCodeGen
{
    public class RazorCodeGenerator : ICodeGenerator<RazorCodeGenInput, string>
    {
        public RazorCodeGenerator()
        {
            Errors = new string[] { };
        }

        public string Output { get; set; }

        public string[] Errors { get; set; }

        public bool Process(RazorCodeGenInput input)
        {
            try
            {
                var template = Template.Compile(input.Template);
                var code = template.Render(input.Model);
                this.Output = code;
                return true;
            }
            catch (Exception ex)
            {
                var list = Errors.ToList();
                list.Add(ex.Message);
                Errors = list.ToArray();
                return false;
            }
        }

    }
}
