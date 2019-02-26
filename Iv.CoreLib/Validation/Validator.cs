using Iv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Iv.Validation
{
    public class Validator
    {
        public const ValidationType RequiredValidationType = ValidationType.Required;
        public const string IdentifierPattern = @"^[a-zA-Z_][a-zA-Z0-9_]*$";
        public const string EmailPattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?"; 
        //@"\b([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})\b";
        public const string PhoneNumberPattern = @"^\+?\d{6,}$";

        public static bool IsIdentifier(string input)
        {
            if(string.IsNullOrEmpty(input)) return false;
            return Regex.IsMatch(input, IdentifierPattern, RegexOptions.IgnoreCase);
        }

        public static bool IsEmail(string input)
        {
            if(string.IsNullOrEmpty(input)) return false;
            return Regex.IsMatch(input, EmailPattern, RegexOptions.IgnoreCase);
        }

        public static bool IsPhoneNumber(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            return Regex.IsMatch(input, PhoneNumberPattern, RegexOptions.IgnoreCase);
        }
    }
}
