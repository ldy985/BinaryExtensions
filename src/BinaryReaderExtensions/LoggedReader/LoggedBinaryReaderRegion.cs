using System;

namespace ldy985.BinaryReaderExtensions.LoggedReader;

/// <summary>Defines a <see cref="LoggedBinaryReader" /> region.</summary>
public readonly record struct LoggedBinaryReaderRegion
{
    /// <summary>Initializes a new instance of <see cref="LoggedBinaryReaderRegion" />.</summary>
    /// <param name="position">The position of the region.</param>
    /// <param name="length">The length for the region.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="position" /> or <paramref name="length" /> are less than
    /// zero.
    /// </exception>
    public LoggedBinaryReaderRegion(long position, long length)
    {
        if (position < 0)
            throw new ArgumentOutOfRangeException(nameof(position));

        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));

        Position = position;
        Length = length;
    }

    /// <summary>Gets the length of this instance.</summary>
    public long Length { get; }

    /// <summary>Gets the position of this instance.</summary>
    public long Position { get; }

    public override string ToString()
    {
        return $"{nameof(Position)}: {Position}, {nameof(Length)}: {Length}";
    }
}