using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace ldy985.BinaryReaderExtensions
{
    /// <summary>Extension methods for <see cref="BinaryReader" />.</summary>
    public static partial class BinaryReaderExtensions
    {
        /// <summary>Reads a structure (see Remarks).</summary>
        /// <param name="reader">The <see cref="BinaryReader" /> to read from.</param>
        /// <typeparam name="T">Structure type.</typeparam>
        /// <returns>The structure read.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader" /> is <c>null</c>.</exception>
        /// <remarks>
        /// The structure will be read using <see cref="Marshal.SizeOf{T}()" /> and
        /// <see cref="Marshal.PtrToStructure{T}(System.IntPtr)" />.
        /// </remarks>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        [MustUseReturnValue]
        public static T ReadStruct<T>(this BinaryReader reader) where T : struct
        {
            int size = Marshal.SizeOf<T>();
            byte[] data = reader.ReadBytes(size);
            return Unsafe.ReadUnaligned<T>(ref data[0]);
        }
    }
}