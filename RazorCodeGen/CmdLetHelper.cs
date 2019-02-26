using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iv.Metadata;
using Iv.Common;
using Iv.Data;

namespace RazorCodeGen
{
    public class CmdLetHelper
    {

        private static CmdLet CreateCmdLet(int index = 0)
        {
            CmdLet cmdLet = new CmdLet()
            {
                Id = Guid.NewGuid(),
                Name = $"Script{index}",
                Path = ""
            };
            cmdLet.Parameters.Add(new CmdLetParameter()
            {
                Id = Guid.NewGuid(),
                Name = "$param1",
                Label = "Param 1",
                Value = "0"
            });
            cmdLet.Parameters.Add(new CmdLetParameter()
            {
                Id = Guid.NewGuid(),
                Name = "$param2",
                Label = "Param 2",
                Value = "ABC"
            });
            cmdLet.Parameters.Add(new CmdLetParameter()
            {
                Id = Guid.NewGuid(),
                Name = "$param3",
                Label = "Param 3",
                Value = "0",
                DataSource = new List<KV<string, string>>
                {
                    new KV<string, string>("0", "First"),
                    new KV<string, string>("1", "Second"),
                    new KV<string, string>("2", "Third"),
                }
            });
            return cmdLet;
        }

        public static void CreateScriptXml(int index = 0)
        {
            var cmdLet = CreateCmdLet(index);
            var fileName = @"Files\Scripts.xml";
            var rep = new XmlFileRepository<CmdLet, Guid>();
            rep.Initialize(fileName);
            rep.Create(cmdLet);
        }

        public static CmdLet LoadScriptXml(string scriptName)
        {
            var fileName = @"Files\Scripts.xml";
            var rep = new XmlFileRepository<CmdLet, Guid>();
            rep.Initialize(fileName);
            var cmdLet = rep.Filter(x => x.Name == scriptName).FirstOrDefault();
            return cmdLet;
        }

        public static void CreateScriptJson(int index = 0)
        {
            var cmdLet = CreateCmdLet(index);
            var fileName = @"Files\Scripts.json";
            var rep = new JsonFileRepository<CmdLet, Guid>();
            rep.Initialize(fileName);
            rep.Create(cmdLet);
        }

        public static CmdLet LoadScriptJson(string scriptName)
        {
            var fileName = @"Files\Scripts.json";
            var rep = new JsonFileRepository<CmdLet, Guid>();
            rep.Initialize(fileName);
            var cmdLet = rep.Filter(x => x.Name == scriptName).FirstOrDefault();
            return cmdLet;
        }

    }
}
