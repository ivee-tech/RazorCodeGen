using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Iv.Runtime
{
    public static class ProcessExtension
    {
        /// <summary>
        /// InputAndOutputToEnd: a handy way to use redirected input/output/error on a p.
        /// </summary>
        /// <param name="p">The p to redirect. Must have UseShellExecute set to false.</param>
        /// <param name="StandardInput">This string will be sent as input to the p. (must be Nothing if not StartInfo.RedirectStandardInput)</param>
        /// <param name="StandardOutput">The p's output will be collected in this ByRef string. (must be Nothing if not StartInfo.RedirectStandardOutput)</param>
        /// <param name="StandardError">The p's error will be collected in this ByRef string. (must be Nothing if not StartInfo.RedirectStandardError)</param>
        /// <remarks>This function solves the deadlock problem mentioned at http://msdn.microsoft.com/en-us/library/system.diagnostics.p.standardoutput.aspx</remarks>
        public static void InputAndOutputToEnd(this System.Diagnostics.Process p, string StandardInput, ref string StandardOutput, ref string StandardError)
        {
            if (p == null)
                throw new ArgumentException("p must be non-null");
            // Assume p has started. Alas there's no way to check.
            if (p.StartInfo.UseShellExecute)
                throw new ArgumentException("Set StartInfo.UseShellExecute to false");
            if ((p.StartInfo.RedirectStandardInput != (StandardInput != null)))
                throw new ArgumentException("Provide a non-null Input only when StartInfo.RedirectStandardInput");
            if ((p.StartInfo.RedirectStandardOutput != (StandardOutput != null)))
                throw new ArgumentException("Provide a non-null Output only when StartInfo.RedirectStandardOutput");
            if ((p.StartInfo.RedirectStandardError != (StandardError != null)))
                throw new ArgumentException("Provide a non-null Error only when StartInfo.RedirectStandardError");
            //
            InputAndOutputToEndData outputData = new InputAndOutputToEndData();
            InputAndOutputToEndData errorData = new InputAndOutputToEndData();
            //
            if (p.StartInfo.RedirectStandardOutput)
            {
                outputData.Stream = p.StandardOutput;
                outputData.Thread = new System.Threading.Thread(InputAndOutputToEndProc);
                outputData.Thread.Start(outputData);
            }
            if (p.StartInfo.RedirectStandardError)
            {
                errorData.Stream = p.StandardError;
                errorData.Thread = new System.Threading.Thread(InputAndOutputToEndProc);
                errorData.Thread.Start(errorData);
            }
            //
            if (p.StartInfo.RedirectStandardInput)
            {
                p.StandardInput.Write(StandardInput);
                p.StandardInput.Close();
            }
            //
            if (p.StartInfo.RedirectStandardOutput) { outputData.Thread.Join(); StandardOutput = outputData.Output; }
            if (p.StartInfo.RedirectStandardError) { errorData.Thread.Join(); StandardError = errorData.Output; }
            if (outputData.Exception != null)
                throw outputData.Exception;
            if (errorData.Exception != null)
                throw errorData.Exception;
        }

        private class InputAndOutputToEndData
        {
            public System.Threading.Thread Thread;
            public System.IO.StreamReader Stream;
            public string Output;
            public Exception Exception;
        }

        private static void InputAndOutputToEndProc(object data_)
        {
            var data = (InputAndOutputToEndData)data_;
            try
            {
                data.Output = data.Stream.ReadToEnd();
            }
            catch (Exception e)
            {
                data.Exception = e;
            }
        }
    }
}
