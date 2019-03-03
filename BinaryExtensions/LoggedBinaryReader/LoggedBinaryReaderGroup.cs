using System;
using JetBrains.Annotations;

namespace SDK.Extensions.LoggedBinaryReader
{
    internal sealed class LoggedBinaryReaderGroup : IDisposable
    {
        public LoggedBinaryReaderGroup([NotNull]LoggedBinaryReader reader)
        {
            Reader = reader;
            reader.BeginGroupInternal();
        }

        private LoggedBinaryReader Reader { get; }

        public void Dispose()
        {
            Reader.EndGroupInternal();
        }
    }
}