using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iv.Validation
{
    public enum ValidationType
    {
	    Required = 1,
	    Range = 2,
	    StringLength = 3,
	    RegularExpression = 4,
	    Editable = 5,
	    Identifier = 6,
        Email = 7
    }
}
