using Iv.Common;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Reflection
{
    public class Evaluator
    {

        public object EvalDefault(ObjectDefProperty mp)
        {
            CodeDomProvider provider = provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;
            cp.TreatWarningsAsErrors = false;
            cp.ReferencedAssemblies.Add(this.GetType().Assembly.Location);
            string key = KeyGenerator.GetString();
            string source = @"
using System;
using Iv.Common;
using Iv.Text;
using Iv.Calendar;
namespace Namespace" + key + @"
{ 
    public class Class" + key + @"
    {
        public static object Execute() 
        { 
            return " + mp.DefaultExpression + @";
        }
    }
}";
            CompilerResults cr = provider.CompileAssemblyFromSource(cp, source);
            if (cr.Errors.Count > 0)
            {
                //TODO: log errors
                foreach (CompilerError ce in cr.Errors)
                {
                    Console.Out.WriteLine("  {0}", ce.ToString());
                }
                return null;
            }
            else
            {
                object obj = cr.CompiledAssembly.CreateInstance(string.Format("Namespace{0}.Class{0}", key));
                MethodInfo mi = obj.GetType().GetMethod("Execute", BindingFlags.Public | BindingFlags.Static);
                var result = mi.Invoke(obj, null);
                mp.Value = result;
                return result;
            }
        }

        public void EvalDefaults(ObjectDef objDef, ObjectDefInstance instance)
        {
            if (objDef == null) return;
            CodeDomProvider provider = provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;
            cp.TreatWarningsAsErrors = false;
            cp.ReferencedAssemblies.Add(this.GetType().Assembly.Location);
            string key = KeyGenerator.GetString();
            StringBuilder sb = new StringBuilder();
            var qProps = objDef.Properties.Where(p => !string.IsNullOrEmpty(p.DefaultExpression));
            foreach (var mp in qProps)
            {
                sb.AppendLine(@"
        public static object Execute" + mp.Name + @"() 
        { 
            return " + mp.DefaultExpression + @";
        }"
                );
            }
            string source = @"
using System; 
using Iv.Common;
using Iv.Text;
using Iv.Calendar;
namespace Namespace" + key + @"
{ 
    public class Class" + key + @"
    {" + sb.ToString() + @"
    }
}";
            CompilerResults cr = provider.CompileAssemblyFromSource(cp, source);
            if (cr.Errors.Count > 0)
            {
                //TODO: log errors
                foreach (CompilerError ce in cr.Errors)
                {
                    Console.Out.WriteLine("  {0}", ce.ToString());
                }
            }
            else
            {
                object obj = cr.CompiledAssembly.CreateInstance(string.Format("Namespace{0}.Class{0}", key));
                MethodInfo mi;
                foreach (var mp in qProps)
                {
                    mi = obj.GetType().GetMethod("Execute" + mp.Name, BindingFlags.Public | BindingFlags.Static);
                    var result = mi.Invoke(obj, null);
                    if(instance.Properties.ContainsKey(mp.Name)) instance.Properties[mp.Name] = result;
                }
            }
        }
    }
}
