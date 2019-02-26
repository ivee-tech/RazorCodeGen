using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Common
{
    public class ObjectDefDbObject : ObjectDefBase<int>
    {
        public int Id { get; set; } 
        public string ObjectDefName { get; set; }
        public string Name { get; set; }
        public ObjectDefDbObjectType Type { get; set; }
    }
}
