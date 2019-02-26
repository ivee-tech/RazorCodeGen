using Iv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Metadata
{
    public class ApplicationInstance : ObjectDefBase<Guid>
    {
        public Guid Id { get; set; }
        // public string Name { get; set; }
        public string Description { get; set; }
        public string Namespace { get; set; }
        public string ConnectionStringName { get; set; }
        public string BaseUrl { get; set; }

        public override Guid Key { get => Id; set => Id = value; }

        public List<FormMeta> Forms { get; set; } = new List<FormMeta>();
        public string SourceDir { get; set; }
    }
}
