using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Iv.Common;

namespace Iv.Reflection
{
    public class PluginRunner
    {

        private PluginCfg cfg = null;

        public PluginRunner()
        {
            cfg = new PluginCfg();
        }

        public PluginRunner(PluginCfg cfg)
            : this()
        {
            this.cfg = cfg;
        }

        public Type GetPlugin(string typeName)
        {
            return Type.GetType(typeName, cfg.ThrowOnError);
        }

        public object CreateInstance(string typeName)
        {
            var t = GetPlugin(typeName);
            var oInstance = Activator.CreateInstance(t, null, null);
            return oInstance;
        }

        public object Execute(object target, string methodName, IEnumerable<KV<string, object>> parameters)
        {
            Type t = target.GetType();
            BindingFlags attr = BindingFlags.Public | BindingFlags.Instance;
            MethodInfo mi = t.GetMethod(methodName, attr);
            if (mi == null)
            {
                if (cfg.ThrowOnError)
                {
                    throw new MissingMethodException(t.FullName, methodName);
                }
                else
                {
                    return null;
                }
            }
            List<object> paramValues = new List<object>();
            foreach(ParameterInfo pi in mi.GetParameters())
            {
                var value = (from p in parameters where p.Key == pi.Name select p.Value).SingleOrDefault();
                paramValues.Add(value);
            }
            var result = mi.Invoke(target, paramValues.ToArray());
            return result;
        }

        public object ExecuteStatic(string typeName, string methodName, IEnumerable<KV<string, object>> parameters)
        {
            Type t = GetPlugin(typeName);
            BindingFlags attr = BindingFlags.Public | BindingFlags.Static;
            MethodInfo mi = t.GetMethod(methodName, attr);
            if (mi == null)
            {
                if (cfg.ThrowOnError)
                {
                    throw new MissingMethodException(t.FullName, methodName);
                }
                else
                {
                    return null;
                }
            }
            List<object> paramValues = new List<object>();
            foreach (ParameterInfo pi in mi.GetParameters())
            {
                var value = (from p in parameters where p.Key == pi.Name select p.Value).SingleOrDefault();
                paramValues.Add(value);
            }
            var result = mi.Invoke(null, paramValues.ToArray());
            return result;
        }

        public object GetValue(object target, string propertyName)
        {
            Type t = target.GetType();
            BindingFlags attrs = BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo pi = t.GetProperty(propertyName, attrs);
            var value = pi.GetValue(target, null);
            return value;
        }

        public object GetStaticValue(string typeName, string propertyName)
        {
            Type t = GetPlugin(typeName);
            BindingFlags attrs = BindingFlags.Public | BindingFlags.Static;
            PropertyInfo pi = t.GetProperty(propertyName, attrs);
            var value = pi.GetValue(null, null);
            return value;
        }

    }
}
