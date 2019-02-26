using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Common
{
    public class ObjectDefView : ObjectDefBase<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Configuration { get; set; }
        public ObjectDefViewType Type { get; set; }
        public string ObjectDefName { get; set; }
    }
}
