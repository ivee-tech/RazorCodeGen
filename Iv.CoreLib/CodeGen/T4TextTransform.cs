using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Diagnostics;

using Iv.Runtime;

namespace Iv.CodeGen
{
    public class T4TextTransform : ICodeGenerator<CodeGenArgs, string>
    {
        private string textTransformFilePath = string.Empty;

        public T4TextTransform(string textTransformFilePath)
        {
            if (string.IsNullOrEmpty(textTransformFilePath))
                throw new ArgumentNullException("textTransformFilePath", "The text transform file path cannot be null or empty.");
            this.textTransformFilePath = textTransformFilePath;
            if (!File.Exists(textTransformFilePath))
                throw new FileNotFoundException("The text transform file path cannot be found", textTransformFilePath);
            Errors = new string[] { };
        }

        public string[] Errors { get; set; }

        public string Output { get; private set; }

        public bool Process(CodeGenArgs args)
        {
            if(!File.Exists(textTransformFilePath))
            {
                Output = string.Empty;
                return false;
            }
            string templateFileName = args.InputFileName;
            if (string.IsNullOrEmpty(templateFileName))
            {
                throw new ArgumentNullException("The template file name cannot be null");
            }

            if (!File.Exists(templateFileName))
            {
                throw new FileNotFoundException("The template file cannot be found", templateFileName);
            }

            string cmdArgs = string.Empty;
            if (string.IsNullOrEmpty(args.OutputFileName))
            {
                throw new ArgumentNullException("OutputFileName", "The output file name cannot be empty.");
            }
            cmdArgs += " -out \"" + args.OutputFileName + "\"";
            for (int i = 0; i < args.Parameters.Count; i++)
            {
                cmdArgs += " -a ";
                cmdArgs += "!!" + args.Parameters[i].Name + "!" + args.Parameters[i].Value + " ";
            }
            cmdArgs += " \"" + templateFileName + "\"";
            ProcessStartInfo psi = new ProcessStartInfo(textTransformFilePath, cmdArgs);
            psi.CreateNoWindow = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            Process p = System.Diagnostics.Process.Start(psi);
            string output = string.Empty;
            string error = string.Empty;
            p.InputAndOutputToEnd(cmdArgs, ref output, ref error);
            p.WaitForExit();
            p.Close();
            if (!string.IsNullOrEmpty(error))
            {
                this.Output = string.Empty;
                error = templateFileName + cmdArgs + Environment.NewLine + error;
                this.Errors = new string[] { error };
                return false;
            }
            this.Output = File.ReadAllText(args.OutputFileName);
            return true;
        }
    }
}
