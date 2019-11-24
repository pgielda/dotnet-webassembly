csc WebAssembly/*.cs WebAssembly/Runtime/*.cs WebAssembly/Runtime/Compilation/*.cs WebAssembly/Instructions/*.cs main.cs -langversion:7.3
echo "List of undefined:"
clang --target=wasm32 -nostdlib -Wl,--no-entry  -Wl,--export-all -o test.wasm test.c 2>&1 | grep undefined | rev | cut -f 1 -d ':' | rev | sort | uniq
echo
echo "Compiling."
clang --target=wasm32 -nostdlib -Wl,--export-all -Wl,--allow-undefined -Wl,--no-entry -o test.wasm test.c

