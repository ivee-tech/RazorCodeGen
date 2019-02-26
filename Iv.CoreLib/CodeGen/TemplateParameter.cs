using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace Iv.CodeGen
{

    public class TemplateParameter
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string Value { get; set; }

        public TemplateParameter() : this("", "")
        {
        }

        public TemplateParameter(string name, string value)
        {
            Name = name;
            Value = value;
            TypeName = "string";
        }

        public TemplateParameter(string name, string typeName, string value)
            : this(name, value)
        {
            TypeName = typeName;
        }

        public TemplateParameter Clone()
        {
            //or MemberWiseClone();
            var p = new TemplateParameter(this.Name, this.TypeName, this.Value);
            return p;
        }
    }

}
