using Iv.Reflection;
using Iv.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Iv.Common
{
    public class ObjectDefInstance : ObjectDefBase<string>
    {
        public string Name { get; set; }
        public KV<string, object> Key { get; set; }
        public Dictionary<string, object> Properties { get; set; }

        public ObjectDefInstance()
        {
            Key = new KV<string, object>("", "");
            Properties = new Dictionary<string, object>();
        }

        public static ObjectDefInstance Create(string name, object values, KV<string, object> key = null)
        {
            var instance = new ObjectDefInstance() { Name = name, Key = key };
            Type type = values.GetType();
            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                instance.Properties.Add(prop.Name, prop.GetValue(values));
            }
            return instance;
        }

        public static ObjectDefInstance CreateInstance<T>(T t, KV<string, object> key = null)
        {
            Type type = typeof(T);
            string name = type.Name;
            string listName = name.Pluralize();
            ObjectDefInstance instance = new ObjectDefInstance()
            {
                Name = type.Name,
                Key = key
            };
            foreach (var p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (p.PropertyType.IsSimpleType())
                {
                    instance.Properties.Add(p.Name, p.GetValue(t));
                }
            }
            return instance;
        }

        public string GetUserDisplay(string userDisplay)
        {
            // TODO: Use ObjectDef.UserDisplay metadata property
            // string userDisplay = ""; // objDef.UserDisplay;
            string pattern = @"\{(?<p>[^\{\}]+)\}";
            MatchCollection coll = Regex.Matches(userDisplay, pattern, RegexOptions.IgnoreCase);
            foreach(Match m in coll)
            {
                if(m.Groups["p"].Success)
                {
                    string propName = m.Groups["p"].Value;
                    if(Properties.ContainsKey(propName))
                    {
                        string sValue = Properties[propName] == null ? "" : Properties[propName].ToString();
                        string patternRepl = @"\{" + propName + @"\}";
                        userDisplay = Regex.Replace(userDisplay, patternRepl, sValue, RegexOptions.IgnoreCase);
                    }
                }
            }
            return userDisplay;
        }


    }
}
