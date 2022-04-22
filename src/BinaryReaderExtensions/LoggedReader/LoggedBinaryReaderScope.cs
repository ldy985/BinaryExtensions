using System;

namespace ldy985.BinaryReaderExtensions.LoggedReader
{
    internal readonly record struct LoggedBinaryReaderScope : IDisposable
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="journal"></param>
        /// <exception cref="InvalidOperationException"></exception>
        internal LoggedBinaryReaderScope(LoggedBinaryReaderJournal journal)
        {
            Journal = journal;
            Journal.BeginLog();
        }

        private LoggedBinaryReaderJournal Journal { get; }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            try
            {
                Journal.EndLog();
            }
            catch (Exception)
            {
                //ignore
            }
        }
    }
}