using System;
using System.IO;
using System.Text;
using JetBrains.Annotations;

namespace ldy985.BinaryReaderExtensions;

public static partial class BinaryReaderExtensions
{
    /// <summary>Read a string with a fixed byte length.</summary>
    /// <param name="br">The binary reader.</param>
    /// <param name="length">The amount of bytes to read and build the string from.</param>
    /// <param name="encoding">The encoding of the string in the bytes.</param>
    /// <returns>The string.</returns>
    /// <exception cref="System.ObjectDisposedException"></exception>
    /// <exception cref="IOException"></exception>
    /// <exception cref="T:System.ArgumentException">The number of decoded characters to read is greater than <paramref name="count" />. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="bytes" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (for more information, see Character Encoding in .NET)
    ///  -and-
    ///  <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
    [MustUseReturnValue]
    public static string ReadFixedString(this BinaryReader br, int length, Encoding encoding)
    {
        return encoding.GetString(br.ReadBytes(length));
    }

    /// <summary>Read a string with a fixed byte length. Optimized for short strings.</summary>
    /// <param name="br">The binary reader.</param>
    /// <param name="length">The amount of bytes to read and build the string from.</param>
    /// <param name="encoding">The encoding of the string in the bytes.</param>
    /// <returns>The string.</returns>
    /// <exception cref="System.ObjectDisposedException"></exception>
    /// <exception cref="IOException"></exception>
    [MustUseReturnValue]
    public static string ReadFixedString(this BinaryReader br, byte length, Encoding encoding)
    {
        Span<byte> bytes = stackalloc byte[length];
        br.ReadBytes(bytes);
        return encoding.GetString(bytes);
    }
}