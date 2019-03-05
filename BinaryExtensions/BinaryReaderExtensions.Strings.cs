using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.Annotations;

namespace SDK.Extensions
{
    public static partial class BinaryReaderExtensions
    {
        /// <summary>
        ///     Peeks a string (see Remarks).
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <param name="encoding">
        ///     The encoding to use.
        /// </param>
        /// <param name="length">
        ///     String length in chars.
        /// </param>
        /// <returns>
        ///     The string.
        /// </returns>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        [NotNull]
        [Pure]
        public static string PeekFixedString([NotNull]this BinaryReader reader, Encoding encoding, int length)
        {
            return reader.Peek(binaryReader => binaryReader.ReadFixedString(length, encoding));
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        [NotNull]
        public static string ReadNullTerminatedString([NotNull]this BinaryReader br)
        {
            List<byte> bytes = new List<byte>();

            byte b;
            while ((b = br.ReadByte()) != 0)
            {
                bytes.Add(b);
            }

            return Encoding.ASCII.GetString(bytes.ToArray());
        }

        [NotNull]
        public static string ReadFixedString([NotNull]this BinaryReader br, long length, [NotNull]Encoding encoding)
        {
            byte[] stringBytes = new byte[length];
            br.Read(stringBytes, 0, stringBytes.Length);

            return encoding.GetString(stringBytes);
        }

        ///// <summary>
        /////     Reads a string.
        ///// </summary>
        ///// <param name="reader">
        /////     The <see cref="BinaryReader" /> to read from.
        ///// </param>
        ///// <param name="encoding"></param>
        ///// <param name="length">
        /////     Length of the string to read in chars.
        ///// </param>
        ///// <returns>
        /////     The string read.
        ///// </returns>
        ///// <exception cref="IOException">An I/O error occurs.</exception>
        //public static string ReadString(this BinaryReader reader, Encoding encoding, int length)
        //{
        //    length *= encoding.GetBytes("\0").Length;

        //    byte[] bytes = reader.ReadBytes(length);

        //    return encoding.GetString(bytes);
        //}

        ///// <summary>
        ///// Reads a string from the current stream. The string is prefixed with the length.
        ///// </summary>
        ///// <param name="reader"></param>
        ///// <returns></returns>
        ///// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        ///// <exception cref="ArgumentOutOfRangeException"><paramref name="count">count</paramref> is negative.</exception>
        ///// <exception cref="IOException">An I/O error occurs.</exception>
        ///// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        ///// <exception cref="ArgumentException">The number of decoded characters to read is greater than <paramref name="count">count</paramref>. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
        //public static string ReadString([NotNull]this BinaryReader reader, Encoding encoding)
        //{
        //    ushort len = reader.ReadUInt16();
        //    len *= (ushort)encoding.GetBytes("\0").Length;

        //    return encoding.GetString(reader.ReadBytes(len));
        //}

        ///// <summary>
        /////     Reads a null-terminated string.
        ///// </summary>
        ///// <param name="reader"></param>
        ///// <returns></returns>
        //public static string ReadStringNullTerminated([NotNull]this BinaryReader reader, Encoding encoding)
        //{
        //    byte[] terminator = encoding.GetBytes("\0"); // Problem: The encoding may not have a NULL character
        //    int charSize = terminator.Length; // Problem: The character size may be variable
        //    List<byte> strBytes = new List<byte>();
        //    byte[] chr;
        //    while (!(chr = reader.ReadBytes(charSize)).SequenceEqual(terminator))
        //    {
        //        if (chr.Length != charSize)
        //            throw new EndOfStreamException();

        //        strBytes.AddRange(chr);
        //    }

        //    return encoding.GetString(strBytes.ToArray());
        //}
    }
}