using Iv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Data
{
    public interface IMetadata<M> where M: class
    {
        void SetMetadata(M model);
    }
}
