using JetBrains.Annotations;
using SDK.Extensions;
using SDK.Extensions.LoggedBinaryReader;
using System;
using System.IO;
using Xunit;

namespace BinaryReaderExtensions.Tests
{
    public class LoggedBinaryReaderTests : IDisposable
    {
        public LoggedBinaryReaderTests()
        {
            testFileReader1 = GetReaderFromFile("../../../TestData/test1.dat");
        }

        public void Dispose()
        {
            testFileReader1.Dispose();
        }

        private readonly LoggedBinaryReader testFileReader1;

        private LoggedBinaryReader GetReaderFromFile([NotNull]string path)
        {
            FileStream fileStream = File.OpenRead(path);
            return new LoggedBinaryReader(fileStream);
        }

        [Fact]
        public void Position()
        {
            Assert.Equal(testFileReader1.BaseStream.Position, testFileReader1.GetPosition());
            Assert.Equal(testFileReader1.BaseStream.Length, testFileReader1.GetLength());

            long startPos = testFileReader1.GetPosition();
            testFileReader1.SkipForwards(2);
            Assert.Equal(startPos + 2, testFileReader1.GetPosition());

            long startPos2 = testFileReader1.GetPosition();
            testFileReader1.SkipBackwards(2);
            Assert.Equal(startPos2 - 2, testFileReader1.GetPosition());

            testFileReader1.SetPosition(0);
            Assert.Equal(0, testFileReader1.GetPosition());

            testFileReader1.SetPosition(23);
            Assert.Equal(23, testFileReader1.GetPosition());

            testFileReader1.Align(16);
            Assert.Equal(32, testFileReader1.GetPosition());

            testFileReader1.Align(16);
            Assert.Equal(48, testFileReader1.GetPosition());

            testFileReader1.SetPosition(1);
            testFileReader1.Align(16);
            Assert.Equal(16, testFileReader1.GetPosition());

            testFileReader1.SetPosition(1);
            testFileReader1.Align(8);
            Assert.Equal(8, testFileReader1.GetPosition());

            testFileReader1.SetPosition(1);
            testFileReader1.Align(32);
            Assert.Equal(32, testFileReader1.GetPosition());

            testFileReader1.SetPosition(24);
            testFileReader1.Align(32);
            Assert.Equal(32, testFileReader1.GetPosition());

            testFileReader1.SetPosition(testFileReader1.GetLength() - 8);
            testFileReader1.Align(16);

            Assert.Empty(testFileReader1.GetReadRegions());

            // Assert.ThrowsAny<Exception>(() => this.testFileReader1.Align(16));
        }

        [Fact]
        public void ReadStringNullTerminated()
        {
            testFileReader1.SetPosition(24);
            Assert.Equal("ANDROI~1", testFileReader1.ReadNullTerminatedString());
        }

        [Fact]
        public void UInt16()
        {
            Assert.Equal(134, testFileReader1.ReadUInt16LE());
            Assert.Equal(29696, testFileReader1.ReadUInt16BE());
        }

        [Fact]
        public void ReadStruct()
        {
            TestStruct readStruct = testFileReader1.ReadStruct<TestStruct>();

            Assert.Equal(134u, readStruct.A);
            Assert.Equal(116u, readStruct.B);
        }

        public struct TestStruct
        {
            public ushort A;
            public ushort B;
        }
    }
}