using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorCodeGen.Models
{
    public class MetaModel
    {
        public string Name { get; set; }
        public IList<Field> Fields { get; private set; }

        public MetaModel()
        {
            this.Fields = new List<Field>();
        }
    }
}
