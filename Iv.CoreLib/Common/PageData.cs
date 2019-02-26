using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iv.Common
{
    public class PageData<T>
    {

        public PageData()
        {
            List = new List<T>();
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int? RecordCount { get; set; }

        public int PageCount
        {
            get
            {
                if (!RecordCount.HasValue) return PageIndex;
                return (int)Math.Ceiling((decimal)RecordCount.Value / PageSize);
            }
        }

        public IEnumerable<T> List { get; set; }
    }
}
