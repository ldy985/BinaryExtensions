using System;
using System.IO;
using JetBrains.Annotations;

namespace ldy985.BinaryReaderExtensions
{
    public static partial class BinaryReaderExtensions
    {
        /// <summary>Read a GUID from 16 bytes.</summary>
        /// <param name="reader"></param>
        /// <returns>The read GUID</returns>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException"></exception>
        [MustUseReturnValue]
        public static Guid ReadGuid(this BinaryReader reader)
        {
            Span<byte> buffer = stackalloc byte[16];
            reader.ReadBytes(buffer);
            return new Guid(buffer);
        }
    }
}