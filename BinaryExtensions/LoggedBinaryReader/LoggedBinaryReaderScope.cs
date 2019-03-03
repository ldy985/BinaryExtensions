using System;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace SDK.Extensions.LoggedBinaryReader
{
    internal struct LoggedBinaryReaderScope : IDisposable
    {
        private LoggedBinaryReaderJournal Journal { get; }

        internal LoggedBinaryReaderScope([NotNull]LoggedBinaryReaderJournal journal)
        {
            Journal = journal;
            Journal.BeginLog();
        }

        public void Dispose()
        {
            Journal.EndLog();
        }
    }
}