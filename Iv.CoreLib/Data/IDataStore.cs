using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Data
{
    public interface IDataStore
    {
        string StoreName { get; }
        void Initialize(string storeName);
    }
}
