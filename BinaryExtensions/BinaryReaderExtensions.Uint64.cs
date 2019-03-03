using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using JetBrains.Annotations;

namespace SDK.Extensions
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static partial class BinaryReaderExtensions
    {
        /// <summary>
        ///     Peeks a 64-bit unsigned integer.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <param name="endianness">
        ///     Integer endianness.
        /// </param>
        /// <returns>
        ///     The integer read.
        /// </returns>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static ulong PeekUInt64([NotNull]this BinaryReader reader, Endianness endianness)
        {
            return reader.Peek(s => s.ReadUInt64(endianness));
        }

        /// <summary>
        ///     Reads a 64-bit unsigned integer.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <param name="endianness">
        ///     Integer endianness.
        /// </param>
        /// <returns>
        ///     The integer read.
        /// </returns>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static ulong ReadUInt64([NotNull]this BinaryReader reader, Endianness endianness)
        {
            ulong value = reader.ReadUInt64();

            switch (endianness)
            {
                case Endianness.BigEndian:
                    return value.ToBigEndian();
                case Endianness.LittleEndian:
                    return value.ToLittleEndian();
                default:
                    throw new ArgumentOutOfRangeException(nameof(endianness), endianness, null);
            }
        }

        /// <summary>
        ///     Reads a 64-bit unsigned integer in big-endian format.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <returns>
        ///     The integer read.
        /// </returns>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        public static ulong ReadUInt64BE([NotNull]this BinaryReader reader)
        {
            return reader.ReadUInt64(Endianness.BigEndian);
        }

        /// <summary>
        ///     Reads a 64-bit unsigned integer in little-endian format.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <returns>
        ///     The integer read.
        /// </returns>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static ulong ReadUInt64LE([NotNull]this BinaryReader reader)
        {
            return reader.ReadUInt64(Endianness.LittleEndian);
        }
    }
}