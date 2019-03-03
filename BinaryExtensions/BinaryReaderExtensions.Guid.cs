using System;
using System.IO;
using JetBrains.Annotations;

namespace SDK.Extensions
{
    public static partial class BinaryReaderExtensions
    {
        /// <summary>
        ///     Read a GUID from 16 bytes.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>The read GUID</returns>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="b">b</paramref> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="b">b</paramref> is not 16 bytes long.</exception>
        public static Guid ReadGuid([NotNull]this BinaryReader reader)
        {
            return new Guid(reader.ReadBytes(16));
        }
    }
}