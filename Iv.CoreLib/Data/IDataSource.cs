﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Iv.Common;

namespace Iv.Data
{
    public interface IDataSource<K, V> : IQuery<KV<K, V>> 
    {

    }
}
