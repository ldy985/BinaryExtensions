using System;
using System.IO;
using System.Text;
using Xunit;

namespace ldy985.BinaryReaderExtensions.Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "UnhandledExceptions:Unhandled exception(s)", Justification = "<Pending>")]
    public partial class BinaryReaderExtensionsTests : TestBase
    {
        /// <summary>Position</summary>
        [Fact]
        public void Position()
        {
            Assert.Equal(TestFileReader1.BaseStream.Position, TestFileReader1.GetPosition());
            Assert.Equal(TestFileReader1.BaseStream.Length, TestFileReader1.GetLength());

            long startPos = TestFileReader1.GetPosition();
            TestFileReader1.SkipForwards(2);
            Assert.Equal(startPos + 2, TestFileReader1.GetPosition());

            long startPos2 = TestFileReader1.GetPosition();
            TestFileReader1.SkipBackwards(2);
            Assert.Equal(startPos2 - 2, TestFileReader1.GetPosition());

            TestFileReader1.SetPosition(0);
            Assert.Equal(0, TestFileReader1.GetPosition());

            TestFileReader1.SetPosition(23);
            Assert.Equal(23, TestFileReader1.GetPosition());

            TestFileReader1.Align(16);
            Assert.Equal(32, TestFileReader1.GetPosition());

            TestFileReader1.Align(16);
            Assert.Equal(48, TestFileReader1.GetPosition());

            TestFileReader1.SetPosition(1);
            TestFileReader1.Align(16);
            Assert.Equal(16, TestFileReader1.GetPosition());

            TestFileReader1.SetPosition(1);
            TestFileReader1.Align(8);
            Assert.Equal(8, TestFileReader1.GetPosition());

            TestFileReader1.SetPosition(1);
            TestFileReader1.Align(32);
            Assert.Equal(32, TestFileReader1.GetPosition());

            TestFileReader1.SetPosition(24);
            TestFileReader1.Align(32);
            Assert.Equal(32, TestFileReader1.GetPosition());

            TestFileReader1.SetPosition(TestFileReader1.GetLength() - 8);
            TestFileReader1.Align(16);

            // Assert.ThrowsAny<Exception>(() => this.testFileReader1.Align(16));
        }

        [Fact]
        public void Peeking()
        {
            TestFileReader1.SetPosition(0);

            byte peek = TestFileReader1.Peek(reader => reader.ReadByte());
            Assert.Equal(0x86, peek);
            Assert.Equal(0, TestFileReader1.GetPosition());

            var data = TestFileReader1.PeekData(8);
            byte[] bytes = { 0x86, 0x00, 0x74, 0x00, 0x1E, 0x00, 0x43, 0x46 };
            Assert.Equal(data, bytes);
            Assert.Equal(0, TestFileReader1.GetPosition());

            TestFileReader1.SetPosition(TestFileReader1.GetLength());

            TestFileReader1.Peek(reader => reader.ReadByte());
        }
    }
}