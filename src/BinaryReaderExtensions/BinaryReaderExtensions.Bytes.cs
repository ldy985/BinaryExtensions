using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

namespace ldy985.BinaryReaderExtensions;

public static partial class BinaryReaderExtensions
{
    /// <summary>Peeks an object at current position (see Remarks).</summary>
    /// <typeparam name="T">Object type.</typeparam>
    /// <param name="reader">The <see cref="BinaryReader" /> to read from.</param>
    /// <param name="func">Function that reads the object.</param>
    /// <returns>The object read.</returns>
    /// <remarks>This method will save underlying stream position and restore it after reading the object.</remarks>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="T:System.Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    [Pure]
    public static T Peek<T>(this BinaryReader reader, Func<BinaryReader, T> func)
    {
        long position = reader.GetPosition();
        T value = func(reader);

        reader.SetPosition(position);

        return value;
    }

    /// <summary>PeekData</summary>
    /// <param name="reader"></param>
    /// <param name="count"></param>
    /// <exception cref="T:System.ArgumentException">The number of decoded characters to read is greater than <paramref name="count" />. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count" /> is negative.</exception>
    [Pure]
    public static byte[] PeekData(this BinaryReader reader, int count)
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
    public static List<byte> ReadBytes(this BinaryReader reader, uint count)
    {
        List<byte> data = new List<byte>();

        while (count > int.MaxValue)
        {
            data.AddRange(reader.ReadBytes(int.MaxValue));
            count -= int.MaxValue;
        }

        return data;
    }

    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public static int ReadBytes(this BinaryReader reader, Span<byte> buffer)
    {
        int numRead = 0;
        int count = buffer.Length;

        do
        {
            int n = reader.BaseStream.Read(buffer.Slice(numRead, count));
            if (n == 0)
                break;

            numRead += n;
            count -= n;
        } while (count > 0);

        return numRead;
    }
}