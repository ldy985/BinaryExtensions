using System;
using System.IO;
using JetBrains.Annotations;

namespace ldy985.BinaryReaderExtensions
{
    public static partial class BinaryReaderExtensions
    {
        /// <summary>ReadFileDateTime</summary>
        /// <param name="br"></param>
        /// <returns></returns>
        /// <exception cref="EndOfStreamException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="IOException"></exception>
        [MustUseReturnValue]
        public static DateTime ReadFileDateTime(this BinaryReader br)
        {
            long value = br.ReadInt64();
            return DateTime.FromFileTimeUtc(value);
        }

        /// <summary>ReadMRUDateTime</summary>
        /// <param name="br"></param>
        /// <returns></returns>
        /// <exception cref="EndOfStreamException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="IOException"></exception>
        [MustUseReturnValue]
        public static DateTime ReadMRUDateTime(this BinaryReader br)
        {
            uint rawDate = br.ReadUInt32();

            if (rawDate == 0)
                return DateTime.MinValue.ToUniversalTime();

            //The day of month is stored in the next 5 bits
            byte day = (byte)(rawDate & 0x1f);
            rawDate >>= 5;

            //The month is stored in the next 4 bits
            byte month = (byte)(rawDate & 0x0f);
            rawDate >>= 4;

            //The year is stored in the next 7 bits starting at 1980
            ushort year = (ushort)(1980 + (rawDate & 0x7f));
            rawDate >>= 7;

            //The number of seconds are stored in the lower 5 bits in intervals of 2 seconds
            byte seconds = (byte)((rawDate & 0x1f) * 2);
            rawDate >>= 5;

            //The number of minutes are stored in the next 6 bits
            byte minutes = (byte)(rawDate & 0x3f);
            rawDate >>= 6;

            //The number of hours are stored in the next 5 bits
            byte hours = (byte)(rawDate & 0x1f);

            return new DateTime(year, month, day, hours, minutes, seconds, DateTimeKind.Utc);
        }

        /// <summary>ReadUnixDatetime</summary>
        /// <param name="br"></param>
        /// <returns></returns>
        /// <exception cref="EndOfStreamException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="IOException"></exception>
        [MustUseReturnValue]
        public static DateTime ReadUnixDatetime(this BinaryReader br)
        {
            long value = br.ReadInt64();
            return DateTimeOffset.FromUnixTimeSeconds(value).UtcDateTime;
        }
    }
}