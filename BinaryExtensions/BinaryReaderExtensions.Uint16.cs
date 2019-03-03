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
        ///     Peeks a 16-bit unsigned integer.
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
        [Pure]
        public static ushort PeekUInt16([NotNull]this BinaryReader reader, Endianness endianness)
        {
            return reader.Peek(s => s.ReadUInt16(endianness));
        }

        /// <summary>
        ///     Reads a 16-bit unsigned integer.
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
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static ushort ReadUInt16(this BinaryReader reader, Endianness endianness)
        {
            ushort value = reader.ReadUInt16();

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
        ///     Reads a 16-bit unsigned integer in big-endian format.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <returns>
        ///     The integer read.
        /// </returns>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static ushort ReadUInt16BE([NotNull]this BinaryReader reader)
        {
            return reader.ReadUInt16(Endianness.BigEndian);
        }

        /// <summary>
        ///     Reads a 16-bit unsigned integer in little-endian format.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <returns>
        ///     The integer read.
        /// </returns>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static ushort ReadUInt16LE([NotNull]this BinaryReader reader)
        {
            return reader.ReadUInt16(Endianness.LittleEndian);
        }
    }
}