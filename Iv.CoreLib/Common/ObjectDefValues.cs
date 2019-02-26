using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Iv.Common
{
    /// <summary>
    /// Lite class containing reduced number of metadata properties and the data values for one item only. 
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class ObjectDefValues : ObjectDefBase<string>
    {

        public ObjectDefValues()
        {
            Values = new Dictionary<string, object>();
            Keys = string.Empty;
        }

        public IDictionary<string, object> Values { get; set; }

        public string Keys { get; set;}

        public void Add(string key, object value)
        {
            Values.Add(key, value);
        }

        public T Map<T>() where T: class
        {
            T o = Activator.CreateInstance<T>();
            var props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach(var prop in props)
            {
                if(this.Values.ContainsKey(prop.Name))
                {
                    prop.SetValue(o, this.Values[prop.Name]);
                }
            }
            return o;
        }

        public void Map<T>(T o) where T: class
        {
            var props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach(var prop in props)
            {
                if(this.Values.ContainsKey(prop.Name))
                {
                    this.Values[prop.Name] = prop.GetValue(o);
                }
                else
                {
                    this.Values.Add(prop.Name, prop.GetValue(o));
                }
            }
        }
    }
}

