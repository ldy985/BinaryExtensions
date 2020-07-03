using System;

namespace ldy985.BinaryReaderExtensions.LoggedReader
{
    /// <summary>Defines a <see cref="LoggedBinaryReader" /> region.</summary>
    public readonly struct LoggedBinaryReaderRegion
    {
        public bool Equals(LoggedBinaryReaderRegion other)
        {
            return Length == other.Length && Position == other.Position;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is LoggedBinaryReaderRegion other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Length.GetHashCode() * 397) ^ Position.GetHashCode();
            }
        }

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
}