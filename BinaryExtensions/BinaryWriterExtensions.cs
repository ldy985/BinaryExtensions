using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using JetBrains.Annotations;

namespace SDK.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="BinaryWriter" />.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [PublicAPI]
    public static partial class BinaryWriterExtensions
    {
        /// <summary>
        ///     Writes a null-terminated ASCII string.
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        public static void WriteStringASCIINullTerminated([NotNull]this BinaryWriter bw, string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            bw.Write(bytes);
            bw.Write(char.MinValue);
        }
    }
}