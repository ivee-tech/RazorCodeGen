using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.CodeGen
{
    public class UpperCaseCodeGenerator : ICodeGenerator<string, string>
    {

        private string output = string.Empty;
        private string[] errors = new string[] { };

        public string[] Errors
        {
            get
            {
                return this.errors;
            }

            set
            {
                this.errors = value;
            }
        }

        public string Output
        {
            get
            {
                return this.output;
            }
        }

        public bool Process(string inputs)
        {
            if (string.IsNullOrEmpty(inputs))
            {
                Array.Resize(ref this.errors, 1);
                this.errors[0] = "The input cannot be empty.";
                return false;
            }
            this.output = inputs.ToUpper();
            return true;
        }
    }
}
