using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Iv.Runtime;
using Iv.Text;

namespace Iv.Reflection
{

    public static class CompileOutputTypeCscExtensions
    {
        public static string AsString(this CompileOutputType compileOutputType)
        {
            switch (compileOutputType)
            {
                case CompileOutputType.ConsoleApplication:
                    return "exe";
                case CompileOutputType.DynamicallyLinkedLibrary:
                    return "library";
                case CompileOutputType.NetModule:
                    return "module";
                case CompileOutputType.WindowsApplication:
                    return "winexe";
                case CompileOutputType.WindowsRuntimeApplication:
                    return "";
                case CompileOutputType.WindowsRuntimeMetadata:
                    return "";
                default:
                    return "exe";
            }
        }
        public static string AsExtension(this CompileOutputType compileOutputType)
        {
            switch (compileOutputType)
            {
                case CompileOutputType.ConsoleApplication:
                    return ".exe";
                case CompileOutputType.DynamicallyLinkedLibrary:
                    return ".dll";
                case CompileOutputType.NetModule:
                    return ".xyz";
                case CompileOutputType.WindowsApplication:
                    return ".exe";
                case CompileOutputType.WindowsRuntimeApplication:
                    return "";
                case CompileOutputType.WindowsRuntimeMetadata:
                    return "";
                default:
                    return ".exe";
            }
        }
    }

    public class RoslynCscCompiler : ICodeCompiler
    {

        private string cscPath = null;

        public RoslynCscCompiler()
        {
            cscPath = ConfigurationManager.AppSettings["CscPath"];
            if(string.IsNullOrEmpty(cscPath))
            {
                throw new ArgumentNullException(nameof(cscPath));
            }
            if(!File.Exists(cscPath))
            {
                throw new FileNotFoundException("File not found.", cscPath);
            }
        }

        public CompileResult CompileCode(string code, CompileOptions options)
        {
            throw new NotImplementedException();
        }

        public CompileResult CompileFiles(IEnumerable<string> files, CompileOptions options)
        {
            if(!files.Any())
            {
                throw new ArgumentException("No source file provided.");
            }
            if(!Directory.Exists(options.OutputDir))
            {
                throw new DirectoryNotFoundException($"Directory {options.OutputDir} not found.");
            }
            var result = new CompileResult();

            string outputPath = Path.Combine(options.OutputDir, $"{options.AssemblyName}{options.OutputType.AsExtension()}");
            string references = options.ReferenceLocations.Aggregate((res, item) => res + "," + item);
            string sourceFiles = files.Aggregate((res, item) => res + " " + item);
            string cmdArgs = $"-target:{options.OutputType.AsString()} -out:{outputPath} -r:{references}";
            if(options.Recurse)
            {
                cmdArgs = $"{cmdArgs} /recurse:{sourceFiles}\\*.cs";
            }
            else
            {
                cmdArgs = $"{sourceFiles} {cmdArgs}";
            }
            var psi = new ProcessStartInfo(cscPath, cmdArgs);
            psi.CreateNoWindow = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            Process p = Process.Start(psi);
            string output = string.Empty;
            string error = string.Empty;
            p.InputAndOutputToEnd(cmdArgs, ref output, ref error);
            p.WaitForExit();
            p.Close();
            if (!string.IsNullOrEmpty(error))
            {
                error = $"{cscPath} {cmdArgs}{Environment.NewLine}{error}";
                result.Errors.Add(error);
                result.Success = false;
            }
            else
            {
                bool hasCompileErrors = output.ContainsCase("error CS");
                if (hasCompileErrors)
                {
                    error = $"{cscPath} {cmdArgs}{Environment.NewLine}{output}";
                    result.Errors.Add(error);
                    result.Success = false;
                }
                else
                {
                    result.Assembly = Assembly.LoadFile(outputPath);
                    result.Success = true;
                }
            }
            return result;
        }

        public ExecuteResult Execute(Assembly assembly, ExecuteOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
