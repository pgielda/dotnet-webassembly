﻿using System.Reflection.Emit;
using WebAssembly.Runtime;
using WebAssembly.Runtime.Compilation;

namespace WebAssembly.Instructions
{
    /// <summary>
    /// Sign-agnostic rotate right.
    /// </summary>
    public class Int32RotateRight : SimpleInstruction
    {
        /// <summary>
        /// Always <see cref="OpCode.Int32RotateRight"/>.
        /// </summary>
        public sealed override OpCode OpCode => OpCode.Int32RotateRight;

        /// <summary>
        /// Creates a new  <see cref="Int32RotateRight"/> instance.
        /// </summary>
        public Int32RotateRight()
        {
        }

        internal sealed override void Compile(CompilationContext context)
        {
            var stack = context.Stack;
            if (stack.Count < 2)
                throw new StackTooSmallException(OpCode.Int32RotateRight, 2, stack.Count);

            var typeB = stack.Pop();
            var typeA = stack.Peek(); //Assuming validation passes, the remaining type will be this.

            if (typeA != WebAssemblyValueType.Int32)
                throw new StackTypeInvalidException(OpCode.Int32RotateRight, WebAssemblyValueType.Int32, typeA);

            if (typeA != typeB)
                throw new StackParameterMismatchException(OpCode.Int32RotateRight, typeA, typeB);

            context.Emit(OpCodes.Call, context[HelperMethod.Int32RotateRight, (helper, c) =>
            {
                var builder = c.ExportsBuilder.DefineMethod(
                    "☣ Int32RotateRight",
                    CompilationContext.HelperMethodAttributes,
                    typeof(uint),
                    new[]
                    {
                            typeof(uint),
                            typeof(int),
                    }
                    );

                var il = builder.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4_S, 31);
                il.Emit(OpCodes.And);
                il.Emit(OpCodes.Shr_Un);

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldc_I4_S, 32);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Sub);
                il.Emit(OpCodes.Ldc_I4_S, 31);
                il.Emit(OpCodes.And);
                il.Emit(OpCodes.Shl);
                il.Emit(OpCodes.Or);

                il.Emit(OpCodes.Ret);
                return builder;
            }
            ]);
        }
    }
}