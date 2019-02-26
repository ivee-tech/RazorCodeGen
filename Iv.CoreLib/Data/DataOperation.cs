using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Data
{
    [Flags]
    public enum DataOperation
    {
        None = 0,
        Create = 1,
        Read = 2,
        Update = 4,
        Delete = 8,
        Other = 16,
        All = Create | Read | Update | Delete | Other
    }
}
