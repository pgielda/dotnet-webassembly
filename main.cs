using System;
using System.IO;

using WebAssembly;
using WebAssembly.Instructions;
using WebAssembly.Runtime;

static class Program
{
    // exported function
    public static void sayc(int value)
    {
	Console.WriteLine("Saying {0}", (char)value);
    }

    static void Main()
    {
        var imports = new ImportDictionary
        {
                    { "env", "sayc", new FunctionImport(new Action<int>(sayc)) },
        };
        var compiled = Compile.FromBinary<dynamic>(new FileStream("test.wasm", FileMode.Open, FileAccess.Read))(imports);
        compiled.Exports.test();
    }
}
