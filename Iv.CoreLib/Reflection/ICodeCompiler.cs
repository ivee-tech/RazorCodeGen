using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Reflection
{
    public interface ICodeCompiler
    {
        CompileResult CompileCode(string code, CompileOptions options);
        CompileResult CompileFiles(IEnumerable<string> files, CompileOptions options);
        ExecuteResult Execute(Assembly assembly, ExecuteOptions options);

    }
}
