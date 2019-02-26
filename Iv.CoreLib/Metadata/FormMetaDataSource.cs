using Iv.Common;
using Iv.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Metadata
{
    public class FormMetaDataSource : ObjectDefBase<Guid>
    {

        public Guid Id { get; set; }
        // public string Name { get; set; }
        public string Description { get; set; }
        public FormMetaDataSourceType Type { get; set; } = FormMetaDataSourceType.KV;
        public string Value { get; set; }
        public string Label { get; set; }
        public string SType
        {
            get
            {
                return Type.ToString();
            }
            set
            {
                FormMetaDataSourceType ft = FormMetaDataSourceType.KV;
                Type = Enum.TryParse<FormMetaDataSourceType>(value, out ft) ? ft : FormMetaDataSourceType.KV;
            }
        }

        public IEnumerable<KV<object, object>> GetValues()
        {
            if (Type != FormMetaDataSourceType.KV)
            {
                throw new Exception($"Applicable to FormMetaDataSourceType.{FormMetaDataSourceType.KV.ToString()} ({(int)FormMetaDataSourceType.KV}) only.");
            }
            var list = new List<KV<object, object>>();
            if (string.IsNullOrEmpty(this.Value))
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
