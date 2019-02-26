using Iv.Common;
using Iv.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Metadata
{
    public interface ICmdLetDomain : IDataStore
    {

        IEnumerable<CmdLet> GetCmdLets();
        CmdLet GetCmdLet(Guid id);
        CmdLet CreateCmdLet(CmdLet cmdLet);
        CmdLet UpdateCmdLet(CmdLet cmdLet);
        void DeleteCmdLet(CmdLet cmdLet);

    }
}
