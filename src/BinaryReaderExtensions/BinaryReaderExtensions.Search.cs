using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

namespace ldy985.BinaryReaderExtensions
{
    public static partial class BinaryReaderExtensions
    {
        /// <summary>Finds the first index of the specified needle. Returns -1 if not found.</summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [MustUseReturnValue]
        public static long IndexOf(this BinaryReader reader, byte[] needle)
        {
            long index = -1;

            using (IEnumerator<long> e = IndexOfAny(reader, needle).GetEnumerator())
            {
                if (e.MoveNext())
                    index = e.Current;
            }

            return index;
        }

        /// <summary>Finds the first index of the specified needle. Returns -1 if not found.</summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [MustUseReturnValue]
        public static long IndexOf(this BinaryReader reader, byte needle)
        {
            long index = -1;

            using (IEnumerator<long> e = IndexOfAny(reader, needle).GetEnumerator())
            {
                if (e.MoveNext())
                    index = e.Current;
            }

            return index;
        }

        /// <summary>Finds all indexes of the specified needle. It returns nothing if there is no match.</summary>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException"></exception>
        [MustUseReturnValue]
        public static IEnumerable<long> IndexOfAny(this BinaryReader reader, byte[] needle)
        {
            if (needle.Length == 0)
                yield break;

            byte[] buffer = new byte[needle.Length];

            long startPosition = reader.GetPosition();

            int i = 0;
            int read;
            while ((read = reader.Read(buffer, 0, needle.Length)) > 0)
            {
                //If we have a smaller buffer than the needle, we can only have a partial match. We need full matches
                //This check also takes care of the end-of-stream case
                if (read != needle.Length)
                    break;

                if (SequenceEqual_I(buffer, needle))
                {
                    reader.SetPosition(startPosition);
                    yield return startPosition + i;
                }

                i++;

                reader.SetPosition(startPosition + i);
            }

            reader.SetPosition(startPosition);
        }

        /// <summary>Finds all indexes of the specified needle. It returns nothing if there is no match.</summary>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException"></exception>
        [MustUseReturnValue]
        public static IEnumerable<long> IndexOfAny(this BinaryReader reader, byte needle)
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
                    reader.SetPosition(startPosition);
                    yield return startPosition + i - 1;
                }

                reader.SetPosition(startPosition + i);
            }

            reader.SetPosition(startPosition);
        }

        /// <summary>Read from the reader until it reaches the specified byte</summary>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException"></exception>
        [MustUseReturnValue]
        public static IEnumerable<byte> ReadUntil(this BinaryReader reader, byte needle)
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
                    reader.SkipBackwards(1);
                    yield break;
                }
            }
        }

        [MustUseReturnValue]
        public static unsafe bool SequenceEqual_I(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            fixed (byte* ptr1 = &array1[0])
            fixed (byte* ptr2 = &array2[0])
            {
                int i = 0;
                int length = array1.Length;
                while (i < length)
                {
                    if (*(ptr1 + i) == *(ptr2 + i))
                        return true;

                    i++;
                }
            }

            return false;
        }
    }
}