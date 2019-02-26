using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Common
{
    public static class IvExtensions
    {
        public static void ForEachReverse<T>(this List<T> list, Action<T> action)
        {
            if(action == null)
            {
                return;
            }
            int i = list.Count - 1;
            while(i >= 0)
            {
                action(list[i]);
                i--;
            }
        }
    }
}
