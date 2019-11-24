using System;
using System.IO;

using WebAssembly;
using WebAssembly.Instructions;
using WebAssembly.Runtime;

using System.Runtime.InteropServices;

static class Program
{
    public static void greet() {
    	Console.WriteLine("hello world!");
    }

    public static int printf(int s, int b) {
    	Console.WriteLine("printf offset={0} values_offset={1}", s, b);
	string st = Marshal.PtrToStringAuto(memory.Start + s);
	Console.WriteLine("printf s='{0}'", st);

        for (int i = 0; i < 255; i++) {
          long val = Marshal.ReadInt32(memory.Start+b+i*4);
          if (val == 0) break;
          Console.WriteLine("argument {0}", val);
        }
	
	return 0;
    }

    static void Main()
    {
        var imports = new ImportDictionary
        {
		    { "env", "greet", new FunctionImport(new Action(greet)) },
		    { "env", "printf", new FunctionImport(new Func<int, int, int>(printf)) },
        };
        var compiled = Compile.FromBinary<dynamic>(new FileStream("test.wasm", FileMode.Open, FileAccess.Read))(imports);
	memory = compiled.Exports.memory;
        compiled.Exports._start();
    }

    static UnmanagedMemory memory;
}
