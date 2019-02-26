using Iv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Metadata
{
    public static class FormMetaFieldExtensions
    {

        public static string GetDotNetPropType(this FormMetaField fld)
        {
            switch (fld.Type)
            {
                case FieldType.number:
                    return "decimal";
                case FieldType.text:
                    return "string";
                case FieldType.date:
                    return "DateTime";
                case FieldType.checkbox:
                    return "bool";
                case FieldType.guid:
                    return "Guid";
                default:
                    return "string";
            }
        }

        public static string GetSqlPropType(this FormMetaField fld)
        {
            switch (fld.Type)
            {
                case FieldType.number:
                    return "DECIMAL(18,2)";
                case FieldType.text:
                    return "NVARCHAR" + (fld.Length.HasValue ? $"({fld.Length.Value})" : "");
                case FieldType.date:
                    return "DATETIME";
                case FieldType.checkbox:
                    return "BIT";
                case FieldType.guid:
                    return "UNIQUEIDENTIFIER";
                default:
                    return "NVARCHAR(MAX)";
            }
        }

        public static object GenerateValue(this FormMetaField fld)
        {
            switch (fld.Type)
            {
                case FieldType.number:
                    return KeyGenerator.GetNumber(1, 999999);
                case FieldType.text:
                    return KeyGenerator.GetString();
                case FieldType.date:
                    return DateTime.Now;
                case FieldType.checkbox:
                    return default(bool);
                case FieldType.guid:
                    return Guid.NewGuid();
                default:
                    return null;
            }
        }

        public static string GenerateValueString(this FormMetaField fld)
        {
            object value = null;
            switch (fld.Type)
            {
                case FieldType.number:
                    value = KeyGenerator.GetNumber(1, 999999);
                    return value.ToString();
                case FieldType.text:
                    value = KeyGenerator.GetString();
                    return $@"""{value}""";
                case FieldType.date:
                    value = DateTime.Now;
                    return $@"DateTime.Parse(""{value}"")";
                case FieldType.checkbox:
                    value = default(bool);
                    return $@"{value}";
                case FieldType.guid:
                    value = Guid.NewGuid();
                    return $@"Guid.Parse(""{value}"")";
                default:
                    return "null";
            }
        }

        public static string GenerateExpression(this FormMetaField fld)
        {
            string value = null;
            switch (fld.Type)
            {
                case FieldType.number:
                    value = "KeyGenerator.GetNumber(1, 999999)";
                    return value;
                case FieldType.text:
                    value = "KeyGenerator.GetString()";
                    return value;
                case FieldType.date:
                    value = "DateTime.Today";
                    return value;
                case FieldType.checkbox:
                    value = "default(bool)";
                    return value;
                case FieldType.guid:
                    value = "Guid.NewGuid()";
                    return value;
                default:
                    return "null";
            }
        }

    }
}
