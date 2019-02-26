using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Iv.Validation
{
    public class IdentifierAttribute : RegularExpressionAttribute
    {

        public IdentifierAttribute()
            : base(Validator.IdentifierPattern)
        {
        }
    }
}
