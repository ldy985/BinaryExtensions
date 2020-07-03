using System.IO;
using JetBrains.Annotations;

namespace ldy985.BinaryWriterExtensions
{
    public static partial class BinaryWriterExtensions
    {
        public static void Write7BitEncodedInt([NotNull]this BinaryWriter bw, int value)
        {
            // Write out an int 7 bits at a time.  The high bit of the byte,
            // when on, tells reader to continue reading more bytes.
            uint v = (uint)value; // support negative numbers
            while (v >= 0x80)
            {
                bw.Write((byte)(v | 0x80));
                v >>= 7;
            }

            bw.Write((byte)v);
        }
    }
}