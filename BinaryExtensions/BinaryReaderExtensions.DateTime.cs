using System;
using System.IO;
using JetBrains.Annotations;

namespace SDK.Extensions
{
    public static partial class BinaryReaderExtensions
    {
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static DateTime ReadUnixDatetime([NotNull]this BinaryReader br)
        {
            long value = br.ReadInt64();
            return DateTimeOffset.FromUnixTimeSeconds(value).DateTime;
        }

        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static DateTime ReadFileDateTime([NotNull]this BinaryReader br)
        {
            long value = br.ReadInt64();
            return DateTime.FromFileTimeUtc(value);
        }

        public static DateTime ReadMRUDateTime([NotNull]this BinaryReader br)
        {
            uint rawDate = br.ReadUInt32();

            if (rawDate == 0)
                return DateTime.MinValue.ToUniversalTime();

            ushort year = 0;
            byte day = 0;
            byte hours = 0;
            byte minutes = 0;
            byte month = 0;
            byte seconds = 0;

            //The day of month is stored in the next 5 bits
            day = (byte)(rawDate & 0x1f);
            rawDate >>= 5;

            //The month is stored in the next 4 bits 
            month = (byte)(rawDate & 0x0f);
            rawDate >>= 4;

            //The year is stored in the next 7 bits starting at 1980
            year = (ushort)(1980 + (rawDate & 0x7f));
            rawDate >>= 7;

            //The number of seconds are stored in the lower 5 bits in intervals of 2 seconds
            seconds = (byte)((rawDate & 0x1f) * 2);
            rawDate >>= 5;

            //The number of minutes are stored in the next 6 bits
            minutes = (byte)(rawDate & 0x3f);
            rawDate >>= 6;

            //The number of hours are stored in the next 5 bits
            hours = (byte)(rawDate & 0x1f);

            return new DateTime(year, month, day, hours, minutes, seconds, DateTimeKind.Utc);
        }
    }
}