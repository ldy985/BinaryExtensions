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
        public static Guid ReadGuid([NotNull]this BinaryReader reader)
        {
            return new Guid(reader.ReadBytes(16));
        }
    }
}