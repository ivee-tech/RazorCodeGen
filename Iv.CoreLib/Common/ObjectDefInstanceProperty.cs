using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Common
{
    [Obsolete("This adds haevy lifting on the instance; let's keep it simple, and make ObjectDefInstance.Properties a IDictionary<string, object>(key, value)")]
    public class ObjectDefInstanceProperty
    {
        private ObjectDefBase<string> _parent;
        private object _value;

        public ObjectDefInstanceProperty()
        {

        }

        public ObjectDefInstanceProperty(ObjectDefBase<string> parent) : this()
        {
            this._parent = parent;
        }

        public ObjectDefBase<string> Parent
        {
            get
            {
                return _parent;
            }
        }

        public string Name { get; set; }

        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                if(_parent != null)
                {
                    _parent.SetDirty();
                }
            }
        }
    }
}
