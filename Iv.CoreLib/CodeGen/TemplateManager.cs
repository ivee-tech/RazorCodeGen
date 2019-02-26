using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace Iv.CodeGen
{

    public class TemplatesManager
    {
        private TemplateNode foundNode = null;
        private string searchPattern = "*.tt";

        public IEnumerable<string> ExcludeFolders { get; private set; }

        public TemplatesManager()
        {
            ExcludeFolders = new string[] {};
        }

        public TemplatesManager(string searchPattern) : this()
        {
            this.searchPattern = searchPattern;
        }

        public TemplatesManager(IEnumerable<string> excludeFolders) : this()
        {
            if (excludeFolders != null)
            {
                this.ExcludeFolders = excludeFolders;
            }
        }

        public TemplatesManager(IEnumerable<string> excludeFolders, string searchPattern) : this(excludeFolders)
        {
            this.searchPattern = searchPattern;
        }

        public TemplateNode LoadTemplates(string folderFullPath, bool recursive = true, bool checkParams = true)
        {            
            TemplateNode root = new TemplateNode();
            root.Name = Path.GetFileNameWithoutExtension(folderFullPath); // "Templates";
            root.Path = string.Empty;
            root.IsFolder = true;
            LoadTemplates(folderFullPath, root, folderFullPath, recursive, checkParams);
            return root;
        }

        public TemplateNode Find(TemplateNode node, string templateKey)
        {
            foundNode = null;
            FindTemplate(node, templateKey);
            return foundNode;
        }

        private void FindTemplate(TemplateNode node, string templateKey)
        {
            if (foundNode != null) return;
            if (node.Key.Equals(templateKey))
            {
                foundNode = node;
            }
            else
                foreach (TemplateNode childNode in node.Nodes)
                {
                    FindTemplate(childNode, templateKey);
                }
        }

        private void LoadTemplates(string folderFullPath, TemplateNode folder, string rootPath, bool recursive = true, bool checkParams = true)
        {
            DirectoryInfo parentDir = new DirectoryInfo(folderFullPath);
            if ((from f in ExcludeFolders where f.ToLower() == parentDir.Name.ToLower() select f).Any())
            {
                return;
            }
            foreach (DirectoryInfo di in parentDir.GetDirectories())
            {
                if ((from f in ExcludeFolders where f.ToLower() == di.Name.ToLower() select f).Any())
                {
                    continue;
                }
                TemplateNode fn = new TemplateNode();
                fn.Name = di.Name;
                fn.Path = di.FullName.Replace(rootPath, string.Empty);
                if (fn.Path.StartsWith("\\")) fn.Path = fn.Path.Substring(1);
                fn.IsFolder = true;
                folder.Nodes.Add(fn);
                if (recursive)
                {
                    LoadTemplates(di.FullName, fn, rootPath, recursive, checkParams);
                }
            }
            foreach (FileInfo fi in parentDir.GetFiles(searchPattern))
            {
                TemplateNode tn = new TemplateNode();
                tn.Name = fi.Name;
                tn.Path = fi.FullName.Replace(rootPath, string.Empty);
                if (tn.Path.StartsWith("\\")) tn.Path = tn.Path.Substring(1);
                tn.IsFolder = false;

                if (checkParams)
                {
                    string content = File.ReadAllText(fi.FullName);
                    Match m;

                    string paramPattern = @"\<\#\s*//Parameters\s*((?<paramType>(string|bool))\s+(?<paramName>\w+);\s*)*\#\>";
                    m = Regex.Match(content, paramPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    List<TemplateParameter> dictItemList = new List<TemplateParameter>();
                    int i = 0;
                    foreach (Capture c in m.Groups["paramName"].Captures)
                    {
                        string typeName = m.Groups["paramType"].Captures[i].Value;
                        TemplateParameter item = new TemplateParameter(c.Value, typeName, "");
                        dictItemList.Add(item);
                        i++;
                    }
                    tn.Parameters = dictItemList.ToArray();
                    string pattern = "\\<\\#\\@\\s*output\\s+extension=\\\"(?<ext>[^\\\"]+)\\\"\\s+fileName=\\\"(?<fn>[^\\\"]+)\\\"\\s*\\#\\>";
                    m = Regex.Match(content, pattern);
                    if (m.Groups["ext"].Success) tn.Extension = m.Groups["ext"].Value;
                    if (m.Groups["fn"].Success) tn.FileName = m.Groups["fn"].Value;
                }

                folder.Nodes.Add(tn);
            }
        }

        public void CopyAll(string sourcePath, string destinationPath, Action<string, string> fileAction = null)
        {
            var srcDirInfo = new DirectoryInfo(sourcePath);
            Directory.CreateDirectory(destinationPath);
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                DirectoryInfo parentDir = new DirectoryInfo(dirPath);
                if ((from f in ExcludeFolders where f.ToLower() == parentDir.Name.ToLower() select f).Any())
                {
                    continue;
                }
                var destPath = Path.Combine(destinationPath, parentDir.Name);
                Directory.CreateDirectory(destPath);
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string srcFilePath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                DirectoryInfo parentDir = new DirectoryInfo(Path.GetDirectoryName(srcFilePath));
                if ((from f in ExcludeFolders where f.ToLower() == parentDir.Name.ToLower() select f).Any())
                {
                    continue;
                }                
                string destFilePath = Path.Combine(destinationPath, srcFilePath.Remove(0, srcDirInfo.FullName.Length + 1));
                File.Copy(srcFilePath, destFilePath, true);
                if(fileAction != null)
                {
                    fileAction(srcFilePath, destFilePath);
                }
            }
        }
    }
}
