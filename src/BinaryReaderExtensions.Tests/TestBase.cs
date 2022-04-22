using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ldy985.BinaryReaderExtensions.Tests
{
    public class TestBase : IDisposable
    {
        protected TestBase()
        {
            TestFileReader1 = GetReaderFromFile("../../../TestData/test1.dat");
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected BinaryReader TestFileReader1 { get; }

        private BinaryReader GetReaderFromFile(string path)
        {
            FileStream fileStream = File.OpenRead(path);
            return new BinaryReader(fileStream);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                TestFileReader1.Dispose();
        }

        /// <inheritdoc />
        ~TestBase()
        {
            Dispose(false);
        }
    }
}