using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace Iv.CodeGen
{


    public class TemplateNode
    {
        private Guid _key = Guid.NewGuid();
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsFolder { get; set; }
        public List<TemplateNode> Nodes { get; private set; }
        public bool IsSelected { get; set; }
        public IEnumerable<TemplateParameter> Parameters { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public string ObjDefName { get; set; }
        public string Key
        {
            get
            {
                //if (Path != null)
                //{
                //    return Regex.Replace(Path, @"\W", "_");
                //}
                return _key.ToString();
            }
            private set { }
        }


        public TemplateNode()
        {
            Name = "";
            ObjDefName = "";
            Path = "";
            Extension = "";
            FileName = "";
            Nodes = new List<TemplateNode>();
            Parameters = new List<TemplateParameter>() { };
        }

        public bool ContainsParam(string paramName)
        {
            foreach (TemplateParameter item in Parameters)
            {
                if (item.Name.Equals(paramName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        public string GetParamValue(string paramName)
        {
            foreach (TemplateParameter item in Parameters)
            {
                if (item.Name.Equals(paramName, StringComparison.InvariantCultureIgnoreCase)) return item.Value;
            }
            return null;

        }

        public TemplateParameter GetParam(string paramName)
        {
            foreach (TemplateParameter item in Parameters)
            {
                if (item.Name.Equals(paramName, StringComparison.InvariantCultureIgnoreCase)) return item;
            }
            return null;

        }

        public TemplateNode Clone()
        {
            //var t = (TemplateNode)this.MemberwiseClone();
            var t = new TemplateNode();
            t.Extension = this.Extension;
            t.FileName = this.FileName;
            t.IsFolder = this.IsFolder;
            t.IsSelected = this.IsSelected;
            t.ObjDefName = this.ObjDefName;
            t.Name = this.Name;
            t.Path = this.Path;
            var list = new List<TemplateParameter>();
            foreach (var p in this.Parameters)
            {
                var newParam = p.Clone();
                list.Add(newParam);
            }
            t.Parameters = list.ToArray();
            foreach (TemplateNode child in this.Nodes)
            {
                var newNode = child.Clone();
                t.Nodes.Add(newNode);
            }
            return t;
        }

        public override string ToString()
        {
            string result = "";
            result += string.Format("Name = {0}; FileName = {1}\r\n", this.Name, this.FileName);
            foreach(var childNode in this.Nodes)
            {
                result += childNode.ToString();
            }
            return result;
        }
    }

}
