using System;
using System.IO;
using System.Text;
using JetBrains.Annotations;

namespace ldy985.BinaryReaderExtensions
{
    public static partial class BinaryReaderExtensions
    {
        /// <summary>Read a string with a fixed byte length.</summary>
        /// <param name="br">The binary reader.</param>
        /// <param name="length">The amount of bytes to read and build the string from.</param>
        /// <param name="encoding">The encoding of the string in the bytes.</param>
        /// <returns>The string.</returns>
        /// <exception cref="System.ObjectDisposedException"></exception>
        /// <exception cref="IOException"></exception>
        [NotNull]
        public static string ReadFixedString([NotNull]this BinaryReader br, int length, [NotNull]Encoding encoding)
        {
            Span<byte> bytes = stackalloc byte[length];
            br.Read(bytes);

            return encoding.GetString(bytes);
        }
    }
}