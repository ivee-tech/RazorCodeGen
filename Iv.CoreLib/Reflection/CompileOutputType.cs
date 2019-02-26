using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Reflection
{
    public enum CompileOutputType // same as Microsoft.CodeAnalysis.OutputKind
    {
        //
        // Summary:
        //     An .exe with an entry point and a console.
        ConsoleApplication = 0,
        //
        // Summary:
        //     An .exe with an entry point but no console.
        WindowsApplication = 1,
        //
        // Summary:
        //     A .dll file.
        DynamicallyLinkedLibrary = 2,
        //
        // Summary:
        //     A .netmodule file.
        NetModule = 3,
        //
        // Summary:
        //     A .winmdobj file.
        WindowsRuntimeMetadata = 4,
        //
        // Summary:
        //     An .exe that can run in an app container.
        //
        // Remarks:
        //     Equivalent to a WindowsApplication, but with an extra bit set in the Portable
        //     Executable file so that the application can only be run in an app container.
        //     Also known as a "Windows Store app".
        WindowsRuntimeApplication = 5
    }
}
