using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using Iv.Common;

namespace Iv.Validation
{
    public class ObjectDefInstanceValidationAttribute
    {
        private ValidationAttribute _attr;
        private ObjectDefInstance _mv;


        public ObjectDefInstanceValidationAttribute(ValidationAttribute attr, ObjectDefInstance mv) : base()
        {
            _attr = attr;
            _mv = mv;
        }

        public bool IsValid(string propertyName)
        {
            if(!_mv.Properties.ContainsKey(propertyName)) return false;
            var value = _mv.Properties[propertyName];
            return _attr.IsValid(value);
        }
    }
}
