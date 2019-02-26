using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace Iv.CodeGen
{

    public class TemplateOutput
    {
        public string DownloadName { get; set; }
        public string DownloadPath { get; set; }
        public string FullName { get; set; }
        public string Extension { get; set; }
        public string Content { get; set; }
        public bool Succeeded { get; set; }
        public string ErrorMessages { get; set; }

        public TemplateOutput()
        {
            DownloadName = "";
            FullName = "";
            Extension = "";
            Content = "";
        }
    }

}
