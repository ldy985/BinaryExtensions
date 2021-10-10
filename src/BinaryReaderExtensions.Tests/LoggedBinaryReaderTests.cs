using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ldy985.BinaryReaderExtensions.LoggedReader;

namespace ldy985.BinaryReaderExtensions.Tests
{
    public class LoggedBinaryReaderTests : IDisposable
    {
        public LoggedBinaryReaderTests()
        {
            _testFileReader1 = GetReaderFromFile("../../../TestData/test1.dat");
        }

        private readonly LoggedBinaryReader _testFileReader1;

        private LoggedBinaryReader GetReaderFromFile([NotNull]string path)
        {
            FileStream fileStream = File.OpenRead(path);
            return new LoggedBinaryReader(fileStream);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _testFileReader1?.Dispose();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        ~LoggedBinaryReaderTests()
        {
            Dispose(false);
        }
    }
}