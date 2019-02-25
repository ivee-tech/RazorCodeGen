using RazorTemplates.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
                var templateString = input.Template;
                var pattern = @"\@model\s+[A-Za-z]+[A-Za-z0-9_\.]*";
                string replacement = "@*$1*@";
                templateString = Regex.Replace(templateString, pattern, replacement);
                var template = Template.Compile(templateString);
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
