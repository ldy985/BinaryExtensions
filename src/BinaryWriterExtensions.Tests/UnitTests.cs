using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;

namespace ldy985.BinaryWriterExtensions.Tests
{
    public class UnitTests : IDisposable
    {
        public UnitTests()
        {
            _testFileReader1 = GetReaderFromFile("../../../TestData/test1.dat");
        }

        private readonly BinaryReader _testFileReader1;

        private BinaryReader GetReaderFromFile([NotNull]string path)
        {
            FileStream fileStream = File.OpenRead(path);
            return new BinaryReader(fileStream);
        }

        [Fact]
        public void UInt16()
        {
            //TODO
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
        ~UnitTests()
        {
            Dispose(false);
        }
    }
}