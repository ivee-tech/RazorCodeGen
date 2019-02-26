using System;
using System.Collections.Generic;
using Iv.Common;

namespace Iv.Metadata
{

    public class CmdLetParameter : ObjectDefBase<Guid>
    {
        public Guid Id { get; set; }
        public new string Name { get; set; }
        public string Value { get; set; }
        public string Label { get; set; }

        public override Guid Key { get => Id; set => Id = value; }
        public List<KV<string, string>> DataSource { get; set; }
    }

}
