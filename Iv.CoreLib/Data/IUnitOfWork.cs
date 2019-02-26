using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Iv.Common;

namespace Iv.Data
{
    public interface IUnitOfWork : IDisposable
    {

        string ConnectionString { get; set; }

        void Initialize();

        void InitSave();

        void Save();

        void CancelSave();

    }
}
