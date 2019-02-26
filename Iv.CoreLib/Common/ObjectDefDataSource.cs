using Iv.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Common
{
    public class ObjectDefDataSource: ObjectDefBase<string>
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public ObjectDefDataSourceType Type { get; set; }
        public string Value { get; set; }
        public string DisplayName { get; set; }

        public IEnumerable<KV<object, object>> GetValues()
        {
            if(Type != ObjectDefDataSourceType.KV)
            {
                throw new Exception("Applicable to ObjectDefDataSourceType.{Text} ({Value}) only.".
                    Interpolate(new { Text = ObjectDefDataSourceType.KV.ToString(), Value = (int)ObjectDefDataSourceType.KV }));
            }
            var list = new List<KV<object, object>>();
            if(string.IsNullOrEmpty(this.Value))
            {
                return list;
            }
            var arrValues = from t in (from p in this.Value.Split(";"[0])
                                       select p.Split(","[0]))
                            select new KV<object, object>(t[0], t[1]);
            return arrValues;
        }

    }
}
