using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using SDK.Helpers;

// ReSharper disable once CheckNamespace
// ReSharper disable once InconsistentNaming
namespace SDK.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="BinaryReader" />.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [PublicAPI]
    public static partial class BinaryReaderExtensions
    {
        #region Objects

        /// <summary>
        ///     Reads a structure (see Remarks).
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <typeparam name="T">
        ///     Structure type.
        /// </typeparam>
        /// <returns>
        ///     The structure read.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="reader" /> is <c>null</c>.
        /// </exception>
        /// <remarks>
        ///     The structure will be read using <see cref="Marshal.SizeOf{T}()" /> and
        ///     <see cref="Marshal.PtrToStructure{T}(System.IntPtr)" />.
        /// </remarks>
        public static T ReadStruct<T>([NotNull]this BinaryReader reader) where T : struct
        {
            int size = Marshal.SizeOf<T>();

            byte[] data = reader.ReadBytes(size);
            using (UnmanagedMemory unmanagedMemory = new UnmanagedMemory(data))
            {
                return Marshal.PtrToStructure<T>(unmanagedMemory);
            }
        }

        /// <summary>
        ///     Peeks an object at current position (see Remarks).
        /// </summary>
        /// <typeparam name="T">
        ///     Object type.
        /// </typeparam>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <param name="func">
        ///     Function that reads the object.
        /// </param>
        /// <returns>
        ///     The object read.
        /// </returns>
        /// <remarks>
        ///     This method will save underlying stream position and restore it after reading the object.
        /// </remarks>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        [Pure]
        public static T Peek<T>([NotNull]this BinaryReader reader, [NotNull]Func<BinaryReader, T> func)
        {
            long position = reader.GetPosition();
            T value = func(reader);

            reader.SetPosition(position);

            return value;
        }

        public static byte[] PeekData([NotNull]this BinaryReader reader, int count)
        {
            byte[] data = reader.ReadBytes(count);
            reader.BaseStream.Position -= count;
            return data;
        }

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


        #endregion

        #region Length

        /// <summary>
        ///     Gets the length of the underlying stream.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <returns>
        ///     The length of the underlying stream.
        /// </returns>
        public static long GetLength([NotNull]this BinaryReader reader)
        {
            return reader.BaseStream.Length;
        }

        /// <summary>
        ///     Sets the length of the underlying stream.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <param name="length">
        ///     Length of the underlying stream.
        /// </param>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException">
        ///     The stream does not support both writing and seeking, such as if the stream is
        ///     constructed from a pipe or console output.
        /// </exception>
        public static void SetLength([NotNull]this BinaryReader reader, long length)
        {
            reader.BaseStream.SetLength(length);
        }

        #endregion

        #region Position

        /// <summary>
        ///     Aligns the position of the underlying stream to a multiple (see Remarks).
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <param name="alignment">
        ///     Number of bytes to align by.
        /// </param>
        /// <remarks>
        ///     The underlying stream position is aligned forward even if already aligned.
        /// </remarks>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static long Align([NotNull]this BinaryReader reader, int alignment)
        {
            long position = reader.GetPosition();
            long remainder = position % alignment;

            reader.SetPosition(position + (alignment - remainder));

            return remainder;
        }

        /// <summary>
        ///     Gets the position of the underlying stream.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <returns>
        ///     The position of the underlying stream.
        /// </returns>
        public static long GetPosition([NotNull]this BinaryReader reader)
        {
            return reader.BaseStream.Position;
        }

        /// <summary>
        ///     Sets the position of the underlying stream.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <param name="position">
        ///     Position to set the underlying stream to.
        /// </param>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static void SetPosition([NotNull]this BinaryReader reader, long position)
        {
            reader.BaseStream.Position = position;
        }

        /// <summary>
        ///     Move underlying stream position forward.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <param name="count">
        ///     Number of bytes to skip.
        /// </param>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static void SkipForwards([NotNull]this BinaryReader reader, long count)
        {
            reader.BaseStream.Position += count;
        }

        /// <summary>
        ///     Move underlying stream position backward.
        /// </summary>
        /// <param name="reader">
        ///     The <see cref="BinaryReader" /> to read from.
        /// </param>
        /// <param name="count">
        ///     Number of bytes to roll.
        /// </param>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static void SkipBackwards([NotNull]this BinaryReader reader, long count)
        {
            reader.BaseStream.Position -= count;
        }

        /// <summary>
        /// Tries to set the position.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static bool TrySetPosition([NotNull]this BinaryReader reader, long pos)
        {
            if (pos < 0)
            {
                return false;
            }

            long length = reader.GetLength();
            if (length - pos >= 0 || reader.GetPosition() + pos <= length)
            {
                reader.SetPosition(pos);
                return true;
            }

            return false;
        }

        #endregion
    }
}