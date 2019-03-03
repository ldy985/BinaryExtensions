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
        ///     Peeks a 32-bit unsigned integer.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <param name="endianness">
        ///     Integer endiannness.
        /// </param>
        /// <returns>
        ///     The integer read.
        /// </returns>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        [Pure]
        public static uint PeekUInt32([NotNull]this BinaryReader reader, Endianness endianness)
        {
            return reader.Peek(s => s.ReadUInt32(endianness));
        }

        /// <summary>
        ///     Reads a 32-bit unsigned integer.
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
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        public static uint ReadUInt32(this BinaryReader reader, Endianness endianness)
        {
            uint value = reader.ReadUInt32();

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
        ///     Reads a 32-bit unsigned integer in big-endian format.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <returns>
        ///     The integer read.
        /// </returns>
        public static uint ReadUInt32BE([NotNull]this BinaryReader reader)
        {
            return reader.ReadUInt32(Endianness.BigEndian);
        }

        /// <summary>
        ///     Reads a 32-bit unsigned integer in little-endian format.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <returns>
        ///     The integer read.
        /// </returns>
        public static uint ReadUInt32LE([NotNull]this BinaryReader reader)
        {
            return reader.ReadUInt32(Endianness.LittleEndian);
        }
    }
}