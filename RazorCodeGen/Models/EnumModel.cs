using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorCodeGen.Models
{
    public class EnumModel
    {
        public string Name { get; set; }
        public IDictionary<string, int> Data { get; private set; }

        public EnumModel()
        {
            this.Data = new Dictionary<string, int>();
        }
    }
}
