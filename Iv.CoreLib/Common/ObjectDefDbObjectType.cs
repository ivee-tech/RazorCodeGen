using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Common
{
    public enum  ObjectDefDbObjectType
    {
        Create = 0,
        Get = 1, // one key only
        Update = 2, // one key only
        Delete = 3, // one key only
        Filter = 4, // must have Input @<condition>, @TopN parameters
        FilterPage = 5, // must have Input @<condition>, @PageNumber, @PageSize parameters and I/O @TotalCount parameter
        GetAll = 6, // must have Input @TopN parameter
        GetChildren = 7 // for related objects, e.g. ObjectDefProperties, ObjectDefDbObjects etc.; must provide the related parent column value, e.g. ObjectDefName
    }
}
