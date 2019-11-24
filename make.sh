csc WebAssembly/*.cs WebAssembly/Runtime/*.cs WebAssembly/Runtime/Compilation/*.cs WebAssembly/Instructions/*.cs main.cs -langversion:7.3
clang --target=wasm32 -nostdlib -Wl,--no-entry  -Wl,--export-all -Wl,--allow-undefined -o test.wasm test.c

