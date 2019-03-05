using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

namespace SDK.Extensions
{
    public static partial class BinaryReaderExtensions
    {
        /// <summary>
        ///     Finds all indexes of the specified needle. It returns nothing if there is no match.
        /// </summary>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static IEnumerable<long> IndexOfAny([NotNull]this BinaryReader reader, [NotNull]byte[] needle)
        {
            if (needle.Length == 0)
                yield break;

            byte[] buffer = new byte[needle.Length];

            long startPosition = reader.GetPosition();

            int i = 0;
            int read;
            while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
            {
                //If we have a smaller buffer than the needle, we can only have a partial match. We need full matches
                //This check also takes care of the end-of-stream case
                if (read != needle.Length)
                    break;

                if (ByteArrayMatch(buffer, needle))
                {
                    SetPosition(reader, startPosition);
                    yield return startPosition + i;
                }

                i++;

                SetPosition(reader, startPosition + i);
            }

            SetPosition(reader, startPosition);
        }

        /// <summary>
        ///     Finds the first index of the specified needle. Returns -1 if not found.
        /// </summary>
        public static long IndexOf([NotNull]this BinaryReader reader, [NotNull]byte[] needle)
        {
            long index = -1;

            using (IEnumerator<long> e = IndexOfAny(reader, needle).GetEnumerator())
            {
                if (e.MoveNext())
                    index = e.Current;
            }

            return index;
        }

        /// <summary>
        ///     Finds all indexes of the specified needle. It returns nothing if there is no match.
        /// </summary>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static IEnumerable<long> IndexOfAny([NotNull]this BinaryReader reader, byte needle)
        {
            //We don't use the BinaryReader.ReadByte() since it will throw an exception at the end of stream.
            //We use Stream.ReadByte() instead, which returns an integer of -1 when end of stream is reached.

            long startPosition = reader.GetPosition();

            int i = 0;
            int readByte;
            while ((readByte = reader.BaseStream.ReadByte()) != -1)
            {
                i++;

                if (readByte == needle)
                {
                    SetPosition(reader, startPosition);
                    yield return startPosition + i - 1;
                }

                SetPosition(reader, startPosition + i);
            }

            SetPosition(reader, startPosition);
        }

        /// <summary>
        ///     Finds the first index of the specified needle. Returns -1 if not found.
        /// </summary>
        public static long IndexOf([NotNull]this BinaryReader reader, byte needle)
        {
            long index = -1;

            using (IEnumerator<long> e = IndexOfAny(reader, needle).GetEnumerator())
            {
                if (e.MoveNext())
                    index = e.Current;
            }

            return index;
        }

        /// <summary>
        ///     Read from the reader until it reaches the specified byte
        /// </summary>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static IEnumerable<byte> ReadUntil([NotNull]this BinaryReader reader, byte needle)
        {
            //We don't use the BinaryReader.ReadByte() since it will throw an exception at the end of stream.
            //We use Stream.ReadByte() instead, which returns an integer of -1 when end of stream is reached.
            int readByte;
            while ((readByte = reader.BaseStream.ReadByte()) != -1)
            {
                if (readByte != needle)
                    yield return (byte)readByte;
                else
                {
                    SkipBackwards(reader, 1);
                    yield break;
                }
            }
        }

        /// <summary>
        ///     This small method assumes the lengths of both a and b are of equal length
        /// </summary>
        [Pure]
        private static bool ByteArrayMatch([NotNull]byte[] a, [NotNull]byte[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;
        }
    }
}