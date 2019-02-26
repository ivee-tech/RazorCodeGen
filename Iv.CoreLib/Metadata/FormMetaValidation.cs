using Iv.Common;
using System;

namespace Iv.Metadata
{

    public class FormMetaValidation : ObjectDefBase<Guid>
    {
        public Guid Id { get; set; } = Guid.Empty;
        public ValidationType Type { get; set; } = ValidationType.none;
        public string Expression { get; set; }
        public string Message { get; set; }
        public Guid FormMetaFieldId { get; set; }
        public string SType
        {
            get
            {
                return Type.ToString();
            }
            set
            {
                ValidationType vt = ValidationType.none;
                Type = Enum.TryParse<ValidationType>(value, out vt) ? vt : ValidationType.none;
            }
        }
    }

    public enum ValidationType {
        none = 0,
        required = 1, // expression: null
        range = 2, // expression: <min>,<max>
        pattern = 3, // expression: Regex
        minLength = 4, // expression: <minLength>
        maxLength = 5, // expression: <maxLength>
        custom = 6
    }
}
