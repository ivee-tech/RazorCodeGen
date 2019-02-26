using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using Iv.Common;
using Iv.Text;

namespace Iv.CodeGen
{
    public class MultiFileCodeGen
    {

        private string templatePath;
        private ICodeGenerator<CodeGenArgs, string> codeGen;
        private IEnumerable<string>  excludeFolders;

        public MultiFileCodeGen(string templatePath, ICodeGenerator<CodeGenArgs, string> codeGen, IEnumerable<string> excludeFolders = null)
        {
            this.templatePath = templatePath;
            this.codeGen = codeGen;
            this.excludeFolders = excludeFolders;
            if (string.IsNullOrEmpty(templatePath))
                throw new ArgumentNullException("templatePath", "Invalid template path name.");
            if (!System.IO.Directory.Exists(templatePath))
                throw new DirectoryNotFoundException("The directory " + templatePath + " does not exist on the server.");
        }

        public TemplateNode GetTemplates()
        {
            TemplatesManager mgr = new TemplatesManager(excludeFolders.IfNotNull(p => p));
            var root = mgr.LoadTemplates(templatePath);
            return root;
        }

        public IEnumerable<TemplateOutput> GenerateCode(string objDefName, IEnumerable<TemplateNode> templateList, bool setContent = false, string outputDir = null)
        {
            if (codeGen == null)
                throw new ArgumentNullException("codeGen", "The code generator object cannot be null.");
            List<TemplateOutput> outputList = new List<TemplateOutput>();
            foreach (TemplateNode node in templateList.Where(t => !t.IsFolder))
            {
                if (node != null)
                {
                    string mn = node.ContainsParam("objDefName") ? node.GetParamValue("objDefName") : objDefName;
                    string downloadPath = Path.GetRandomFileName();
                    string tempDir = outputDir;
                    if(string.IsNullOrEmpty(tempDir)) tempDir = Path.Combine(Path.GetTempPath(), downloadPath);
                    Directory.CreateDirectory(tempDir);
                    string fileName = Path.GetFileNameWithoutExtension(node.Path);
                    if (!string.IsNullOrEmpty(node.FileName))
                    {
                        fileName = Regex.Replace(node.FileName, @"\$ObjDef\$", mn, RegexOptions.IgnoreCase);
                        fileName = Regex.Replace(fileName, @"\$ObjDefList\$", mn.Pluralize(), RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        string pattern = @"(?<pre>\w*)(?<objDef>ObjDef)(?<name>\w*)(?<template>Template)";
                        fileName = Regex.Replace(fileName, pattern, "${pre}" + mn + "${name}" + node.Extension, RegexOptions.IgnoreCase);
                    }
                    downloadPath = Path.Combine(downloadPath, fileName);
                    string outputFileName = Path.Combine(tempDir, fileName);
                    var output = GenCode(codeGen, node, outputFileName, downloadPath);
                    if (setContent)
                    {
                        output.Content = File.ReadAllText(outputFileName);
                    }
                    outputList.Add(output);
                }
            }
            return outputList;
        }

        public IEnumerable<TemplateOutput> GenerateCode(IEnumerable<string> objDefNames, IEnumerable<TemplateNode> templateList, bool setContent = false)
        {
            List<TemplateOutput> list = new List<TemplateOutput>();            
            foreach (string modelName in objDefNames)
            {
                var q = templateList.Where(t => modelName.Equals(t.ObjDefName));
                list.AddRange(GenerateCode(modelName,  q, setContent));
            }
            return list;
        }

        private TemplateOutput GenCode(ICodeGenerator<CodeGenArgs, string> codeGen, TemplateNode node, string outputFileName, string downloadPath)
        {
            string inputFileName = Path.Combine(templatePath, node.Path);
            TemplateOutput output = new TemplateOutput();
            FileInfo fiOut = new FileInfo(outputFileName);
            output.DownloadName = fiOut.Name;
            output.Extension = fiOut.Extension;
            CodeGenArgs args = new CodeGenArgs();
            args.InputFileName = inputFileName;
            args.OutputFileName = outputFileName;
            foreach (TemplateParameter param in node.Parameters)
                args.Parameters.Add(param);
            if (codeGen.Process(args))
            {
                output.FullName = string.Empty; // outputFileName;
                output.Succeeded = true;
                FileInfo fi = new FileInfo(outputFileName);
                output.DownloadName = fi.Name;
                output.DownloadPath = downloadPath;
                output.Content = codeGen.Output;
            }
            else
            {
                output.Succeeded = false;
                output.FullName = outputFileName;
                string errorMessages = string.Empty;
                foreach (string error in codeGen.Errors)
                {
                    errorMessages += error + "\r\n";
                }
                output.ErrorMessages = errorMessages;
            }
            return output;
        }

    }
}
