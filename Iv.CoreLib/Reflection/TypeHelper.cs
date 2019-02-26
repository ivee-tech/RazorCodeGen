using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Iv.Reflection
{
    public static class TypeHelper
    {

        public static Type GetUnderlyingType(this Type t)
        {
            if (t == null)
                return null;
            return Nullable.GetUnderlyingType(t) ?? t;
        }

        public static Type GetUnderlyingType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return null;
            Type t = Type.GetType(typeName, false);
            return t.GetUnderlyingType();
        }

        public static bool IsSimpleType(this Type t)
        {
            if (t == null) return false;
            var rt = t.GetUnderlyingType();
            return rt.IsPrimitive || rt.IsEnum 
                || "System.String".Equals(rt.FullName, StringComparison.InvariantCultureIgnoreCase)
                || "System.DateTime".Equals(rt.FullName, StringComparison.InvariantCultureIgnoreCase)
                || "System.Decimal".Equals(rt.FullName, StringComparison.InvariantCultureIgnoreCase);
        }

        public static IEnumerable<PropertyInfo> GetInstanceProperties(this Type t)
        {
            var props = from prop in t.GetProperties(BindingFlags.Instance | BindingFlags.Public) select prop;
            return props;
        }

        public static IEnumerable<PropertyInfo> GetInstanceProperties<AttributeType>(this Type t)
        {
            var props = t.GetInstanceProperties();
            foreach (var prop in props)
            {
                var qattrs = from attr in prop.CustomAttributes
                             where attr.AttributeType == typeof(AttributeType)
                             select attr;
                if (qattrs.Any())
                { 
                    yield return prop;
                }
            }
        }

        public static IEnumerable<PropertyInfo> GetInstanceBrowsableProperties(this Type t)
        {
            var props = t.GetInstanceProperties();
            foreach (var prop in props)
            {
                var qattrs1 = from attr1 in prop.CustomAttributes select attr1;
                if (!qattrs1.Any())
                {
                    yield return prop;
                }
                var qattrs2 = from attr2 in prop.GetCustomAttributes()
                              where attr2.GetType() == typeof(BrowsableAttribute) && ((BrowsableAttribute)attr2).Browsable
                              select attr2;
                if (qattrs2.Any())
                { 
                    yield return prop;
                }
                var qattrs3 = from attr3 in prop.GetCustomAttributes()
                              where attr3.GetType() != typeof(BrowsableAttribute)
                              select attr3;
                if (qattrs3.Any())
                {
                    yield return prop;
                }
            }
        }

        public static Type GetFullType(this Type t, string typeName)
        {
            if(string.IsNullOrEmpty(typeName))
            {
                return null;
            }
            if (typeName.StartsWith("System.", StringComparison.InvariantCultureIgnoreCase))
            {
                return Type.GetType(typeName);
            }
            return Type.GetType("System." + typeName);

        }
    }
}
