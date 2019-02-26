using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iv.Metadata;
using RazorCodeGen.Customers;
using RazorCodeGen.Models;

namespace RazorCodeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var meta = new MetaModel { Name = "Person" };
            meta.Fields.Add(new Field() { Name = "Id", Type = FieldTypes.guid });
            meta.Fields.Add(new Field() { Name = "Name", Type = FieldTypes.text });
            meta.Fields.Add(new Field() { Name = "Dob", Type = FieldTypes.date });
            var output = GenerateCode<MetaModel>(meta, @"Templates\Model.cshtml");
            if(output.Result)
                Console.WriteLine(output.Data);
            else
                Console.WriteLine(output.Message);

            var enumModel = new EnumModel() { Name = "Currency" };
            enumModel.Data.Add("AUD", 0);
            enumModel.Data.Add("USD", 1);
            enumModel.Data.Add("EUR", 2);
            enumModel.Data.Add("GBP", 3);
            output = GenerateCode<EnumModel>(enumModel, @"Templates\Enum.cshtml");
            if (output.Result)
                Console.WriteLine(output.Data);
            else
                Console.WriteLine(output.Message);

            int index = new Random().Next(1, 999);
            string scriptName = $"Script{index}";
            CmdLetHelper.CreateScriptXml(index);
            var cmdLet = CmdLetHelper.LoadScriptXml(scriptName);
            output = GenerateCode<CmdLet>(cmdLet, @"Templates\Script.cshtml");
            if (output.Result)
                Console.WriteLine(output.Data);
            else
                Console.WriteLine(output.Message);

            var cmdHelper = new CustomerDataCommandHelper();
            var c = cmdHelper.Create($"Contoso-{new Random().Next(1, 9999)}");
            c = cmdHelper.UpdateWithSpec(c.Id);
            var qryHelper = new CustomerDataQueryHelper();
            c = qryHelper.Find(c.Id);
            var list = qryHelper.GetAll().ToList();
            output = GenerateCode(list, @"Templates\Customers.cshtml");
            if (output.Result)
                Console.WriteLine(output.Data);
            else
                Console.WriteLine(output.Message);


            Console.ReadKey();
        }

        private static OutputModel<string> GenerateCode<T>(T inputModel, string templateFilePath) where T : new()
        {
            var output = new OutputModel<string>();

            ICodeGenerator<RazorCodeGenInput, string> codeGen = new RazorCodeGenerator();
            var input = new RazorCodeGenInput()
            {
                Template = System.IO.File.ReadAllText(templateFilePath),
                Model = inputModel
            };

            var result = codeGen.Process(input);
            if (!result)
            {
                var sb = new StringBuilder();
                foreach (string error in codeGen.Errors)
                {
                    sb.AppendLine(error);
                }
                output.Message = sb.ToString();
                output.Result = false;
            }
            else
            {
                output.Data = codeGen.Output;
                output.Result = true;
            }
            return output;
        }

    }
}