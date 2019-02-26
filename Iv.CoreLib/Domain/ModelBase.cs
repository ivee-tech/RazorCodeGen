using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Iv.Common;

namespace Iv.Domain
{
    public class ModelBase
    {
        public ModelBase()
        {

        }

        public ModelBase(ObjectDefBase<string> objectDef) : this()
        {
            this.ObjectDef = objectDef;
        }

        public ObjectDefBase<string> ObjectDef { get; set; }
    }
}
