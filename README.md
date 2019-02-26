# RazorCodeGen - code generator using Razor Templates

Generate code based on your models and Razor templates.
The code uses the [Razor Templates](https://github.com/volkovku/RazorTemplates) Nuget package.

## Example

Let's say we have a model called <code>EnumModel</code> with two properties: <code>string Name</code> and <code>IDictionary<string, int> Data</code>.
We want to generate an enum named using the <code>Name</code> property and the values based om <code>Data</code> dictionary.

The <code>EnumModel</code> class is shown below:
``` C#
    public class EnumModel
    {
        public string Name { get; set; }
        public IDictionary<string, int> Data { get; private set; }

        public EnumModel()
        {
            this.Data = new Dictionary<string, int>();
        }
    }

```

Using a simple Razor template bound to an instance of our model, we can generate the enum.
This is the template code (from *Enum.cshtml*):

``` C#
@using RazorCodeGen.Models
@model EnumModel
@{
    var ns = "Models";
}
using System;
using Iv.Common;

namespace @(ns)
{

    public enum @Model.Name
    {
    @foreach (var kvp in Model.Data)
    {
        <text>@(kvp.Key) = @(kvp.Value),</text>
    }
    }

}
```

Say we want to generate a Currency enumeration, with a couple of values: AUD, USD, EUR, GBP.
This is the generation code:

``` C#
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
```

And this is the output:

``` C#
using System;

namespace Models
{

    public enum Currency
    {
        AUD = 0,
        USD = 1,
        EUR = 2,
        GBP = 3,
    }

}
```

The <code>GenerateCode</code> method is displayed partially  below:
``` C#
    var output = new OutputModel<string>();

    ICodeGenerator<RazorCodeGenInput, string> codeGen = new RazorCodeGenerator();
    var input = new RazorCodeGenInput()
    {
        Template = System.IO.File.ReadAllText(templateFilePath),
        Model = inputModel
    };

    var result = codeGen.Process(input);
```

Other examples include:
 - generate a class based on a simple metadata model (<code>MetaModel</code> class and *Model.cshtml* template)
 - generate a PowerShell script based on a specific cmdlet metadata, stored in an Xml file (<code>CmdLet</code> class and *Script.cshtml* template)
 - generate a customers HTML table, based on a list of customers stored in a SQL Server DB (<code>Customer</code> class and *Customers.cshtml* template)
