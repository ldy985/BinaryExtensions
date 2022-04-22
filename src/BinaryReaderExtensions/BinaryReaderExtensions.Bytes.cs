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
    /// <exception cref="IOException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
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