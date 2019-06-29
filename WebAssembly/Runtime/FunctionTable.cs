﻿using System;

namespace WebAssembly.Runtime
{
    /// <summary>
    /// An array-like structure representing a table import/export, which for the initial specification of WebAssembly is always a function list.
    /// Its behavior mimics the JavaScript version, where the import object is actually modified by the instantiation process.
    /// </summary>
    public class FunctionTable : TableImport
    {
        /// <summary>
        /// Always <see cref="ElementType.AnyFunction"/>.
        /// </summary>
        public sealed override ElementType Element => ElementType.AnyFunction;

        /// <summary>
        /// The initial number of elements.
        /// </summary>
        public uint Initial { get; }

        /// <summary>
        /// The maximum number of elements the table is allowed to grow to.
        /// </summary>
        public uint? Maximum { get; }

        /// <summary>
        /// Creates a new <see cref="FunctionTable"/> with the provided initial and maximum size.
        /// </summary>
        /// <param name="initial">The initial number of elements.</param>
        /// <param name="maximum">The maximum number of elements the table is allowed to grow to.</param>
        /// <exception cref="ArgumentException"><paramref name="initial"/> cannot exceed <paramref name="maximum"/>.</exception>
        public FunctionTable(uint initial, uint? maximum = null)
        {
            if (initial > maximum.GetValueOrDefault(uint.MaxValue))
                throw new ArgumentException("initial cannot exceed maximum.", nameof(initial));

            this.Initial = initial;
            this.Maximum = maximum;
            this.delegates = new Delegate[initial];
        }

        private Delegate[] delegates;

        /// <summary>
        /// Gets or sets the delegate at the indicated index.
        /// </summary>
        /// <param name="index">The index within the table to target.</param>
        /// <returns>The delegate at that index, which may be null.</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> does not fall within the range of the table.</exception>
        public Delegate this[int index]
        {
            get => this.delegates[index];
            set => this.delegates[index] = value;
        }

        /// <summary>
        /// Gets the current size of the table.
        /// </summary>
        public uint Length => (uint)this.delegates.Length;

        /// <summary>
        /// Increases the size of the instance by a specified number of elements.
        /// </summary>
        /// <param name="number">The number of elements you want to grow the table by.</param>
        /// <returns>The previous length of the table.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="number"/> must be at least 0 and, when added to <see cref="Length"/>, cannot exceed <see cref="Maximum"/>.
        /// </exception>
        public uint Grow(uint number)
        {
            var count = this.Length;
            var newSize = count + number;

            if (newSize > this.Maximum.GetValueOrDefault(uint.MaxValue))
                throw new ArgumentOutOfRangeException(nameof(number), "Adding number to the current Count would exceed the defined Maximum.");

            Array.Resize(ref delegates, checked((int)newSize));

            return count;
        }
    }
}