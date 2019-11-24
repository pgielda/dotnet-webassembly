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
/*
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
*/
    public static int fd_close(int a) {
    Console.WriteLine("a");
    return 0;
    }
    public static int fd_fdstat_get(int a, int b) {
    Console.WriteLine("b");
    return 0;
    }


    public static int fd_seek(int a, long b, int c, int d) {
    Console.WriteLine("c");
    return 0;
    }


    public static int fd_write(int a, int b, int c, int d) {
    Console.WriteLine("d");
    return 0;
    }

    static void Main()
    {
        var imports = new ImportDictionary
        {
		    { "env", "greet", new FunctionImport(new Action(greet)) },
//		    { "env", "printf", new FunctionImport(new Func<int, int, int>(printf)) },
		    { "wasi_unstable", "fd_close", new FunctionImport(new Func<int, int>(fd_close)) },
		    { "wasi_unstable", "fd_seek", new FunctionImport(new Func<int, long, int, int, int>(fd_seek)) },
                    { "wasi_unstable", "fd_write", new FunctionImport(new Func<int, int, int, int, int>(fd_write)) },
		    { "wasi_unstable", "fd_fdstat_get",  new FunctionImport(new Func<int, int, int>(fd_fdstat_get)) },


        };
        var compiled = Compile.FromBinary<dynamic>(new FileStream("test.wasm", FileMode.Open, FileAccess.Read))(imports);
	Console.WriteLine("Going to execute...");
	memory = compiled.Exports.memory;
        compiled.Exports._start();
    }

    static UnmanagedMemory memory;
}
