using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iv.Common
{
    public class KV<K, V>
    {
        public K Key { get; set; }
        public V Value { get; set; }

        public KV()
        {
        }

        public KV(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }
}
