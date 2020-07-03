using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ldy985.BinaryReaderExtensions
{
    /// <summary>Extension methods for <see cref="BinaryReader" />.</summary>
    [PublicAPI]
    public static partial class BinaryReaderExtensions
    {
        #region Objects

        /// <summary>Peeks an object at current position (see Remarks).</summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="reader">The <see cref="BinaryReader" /> to read from.</param>
        /// <param name="func">Function that reads the object.</param>
        /// <returns>The object read.</returns>
        /// <remarks>This method will save underlying stream position and restore it after reading the object.</remarks>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        [Pure]
        public static T Peek<T>([NotNull]this BinaryReader reader, [NotNull]Func<BinaryReader, T> func)
        {
            long position = reader.GetPosition();
            T value = func(reader);

            reader.SetPosition(position);

            return value;
        }

        /// <summary>PeekData</summary>
        /// <param name="reader"></param>
        /// <param name="count"></param>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        [Pure]
        public static byte[] PeekData([NotNull]this BinaryReader reader, int count)
        {
            byte[] data = reader.ReadBytes(count);
            reader.SkipBackwards(count);

            return data;
        }

        /// <summary>ReadBytes</summary>
        /// <param name="reader"></param>
        /// <param name="count"></param>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static List<byte> ReadBytes([NotNull]this BinaryReader reader, uint count)
        {
            List<byte> data = new List<byte>();
            while (count > int.MaxValue)
            {
                data.AddRange(reader.ReadBytes(int.MaxValue));
                count -= int.MaxValue;
            }

            return data;
        }

        #endregion Objects

        #region Length

        /// <summary>Gets the length of the underlying stream.</summary>
        /// <param name="reader">The <see cref="BinaryReader" /> to read from.</param>
        /// <returns>The length of the underlying stream.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetLength([NotNull]this BinaryReader reader)
        {
            return reader.BaseStream.Length;
        }

        /// <summary>Sets the length of the underlying stream.</summary>
        /// <param name="reader">The <see cref="BinaryReader" /> to read from.</param>
        /// <param name="length">Length of the underlying stream.</param>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException">
        /// The stream does not support both writing and seeking, such as if the stream is
        /// constructed from a pipe or console output.
        /// </exception>
        /// <exception cref="ObjectDisposedException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLength([NotNull]this BinaryReader reader, long length)
        {
            reader.BaseStream.SetLength(length);
        }

        #endregion Length

        #region Position

        /// <summary>Aligns the position of the underlying stream to a multiple (see Remarks).</summary>
        /// <param name="reader">The <see cref="BinaryReader" /> to read from.</param>
        /// <param name="alignment">Number of bytes to align by.</param>
        /// <remarks>The underlying stream position is aligned forward even if already aligned.</remarks>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static long Align([NotNull]this BinaryReader reader, int alignment)
        {
            long position = reader.GetPosition();
            long remainder = position % alignment;

            reader.SetPosition(position + (alignment - remainder));

            return remainder;
        }

        /// <summary>Gets the position of the underlying stream.</summary>
        /// <param name="reader">The <see cref="BinaryReader" /> to read from.</param>
        /// <returns>The position of the underlying stream.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static long GetPosition([NotNull]this BinaryReader reader)
        {
            return reader.BaseStream.Position;
        }

        /// <summary>Sets the position of the underlying stream.</summary>
        /// <param name="reader">The <see cref="BinaryReader" /> to read from.</param>
        /// <param name="position">Position to set the underlying stream to.</param>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPosition([NotNull]this BinaryReader reader, long position)
        {
            reader.BaseStream.Position = position;
        }

        /// <summary>Move underlying stream position backward.</summary>
        /// <param name="reader">The <see cref="BinaryReader" /> to read from.</param>
        /// <param name="count">Number of bytes to roll.</param>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SkipBackwards([NotNull]this BinaryReader reader, long count)
        {
            reader.BaseStream.Position -= count;
        }

        /// <summary>Move underlying stream position forward.</summary>
        /// <param name="reader">The <see cref="BinaryReader" /> to read from.</param>
        /// <param name="count">Number of bytes to skip.</param>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SkipForwards([NotNull]this BinaryReader reader, long count)
        {
            reader.BaseStream.Position += count;
        }

        /// <summary>Tries to set the position.</summary>
        /// <param name="reader"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        /// <exception cref="IOException"></exception>
        public static bool TrySetPosition([NotNull]this BinaryReader reader, long pos)
        {
            if (pos < 0)
                return false;

            long length = reader.GetLength();

            if (length - pos < 0 && reader.GetPosition() + pos > length)
                return false;

            reader.SetPosition(pos);
            return true;
        }

        #endregion Position
    }
}