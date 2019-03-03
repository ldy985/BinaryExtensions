using System;
using System.IO;
using JetBrains.Annotations;

namespace SDK.Extensions
{
    public static partial class BinaryReaderExtensions
    {
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static T ReadEnum<T>([NotNull]this BinaryReader br, bool failOnInvalidValue = false) where T : struct, Enum
        {
            Type type = typeof(T);
            Type underlyingType = type.GetEnumUnderlyingType();

            object value;

            if (underlyingType == typeof(byte))
            {
                value = br.ReadByte();
            }
            else if (underlyingType == typeof(short))
            {
                value = br.ReadInt16();
            }
            else if (underlyingType == typeof(ushort))
            {
                value = br.ReadUInt16();
            }
            else if (underlyingType == typeof(int))
            {
                value = br.ReadInt32();
            }
            else if (underlyingType == typeof(uint))
            {
                value = br.ReadUInt32();
            }
            else if (underlyingType == typeof(long))
            {
                value = br.ReadInt64();
            }
            else if (underlyingType == typeof(ulong))
            {
                value = br.ReadUInt64();
            }
            else
            {
                throw new NotSupportedException("Only certain value types are permitted.");
            }

            T enumValue = default;

            try
            {
                enumValue = (T)value;
            }
            catch (InvalidCastException)
            {
                //Ignore
            }

            if (failOnInvalidValue && !Enum.IsDefined(typeof(T), enumValue))
            {
                throw new Exception($"Invalid value '{value}' for type '{type.Name}'");
            }

            return enumValue;
        }
    }
}