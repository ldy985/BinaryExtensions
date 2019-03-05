using System;
using System.IO;
using JetBrains.Annotations;

namespace SDK.Extensions
{
    public static partial class BinaryReaderExtensions
    {
        /// <exception cref="FormatException">Format_Bad7BitInt32</exception>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        public static int Read7BitEncodedInt([NotNull]this BinaryReader br)
        {
            // Read out an Int32 7 bits at a time.  The high bit
            // of the byte when on means to continue reading more bytes.
            int count = 0;
            int shift = 0;
            byte b;
            do
            {
                // Check for a corrupted stream.  Read a max of 5 bytes.
                // In a future version, add a DataFormatException.
                if (shift == 5 * 7) // 5 bytes max per Int32, shift += 7
                    throw new FormatException("Format_Bad7BitInt32");

                // ReadByte handles end of stream cases for us.
                b = br.ReadByte();
                count |= (b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);

            return count;
        }
    }
}