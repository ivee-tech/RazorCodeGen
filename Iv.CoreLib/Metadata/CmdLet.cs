using System;
using System.Collections.Generic;
using Iv.Common;

namespace Iv.Metadata
{

    public class CmdLet : ObjectDefBase<Guid>
    {
        public Guid Id { get; set; }
        public new string Name { get; set; }
        public string Path { get; set; }

        public override Guid Key { get => Id; set => Id = value; }
        public List<CmdLetParameter> Parameters { get; private set; } = new List<CmdLetParameter>();
    }

}
