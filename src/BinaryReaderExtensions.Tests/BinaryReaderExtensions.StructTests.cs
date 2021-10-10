using Xunit;

namespace ldy985.BinaryReaderExtensions.Tests
{
    public partial class BinaryReaderExtensionsTests
    {
        private struct TestStruct
        {
            public ushort A;
            public ushort B;
        }

        /// <summary>
        /// ReadStruct
        /// </summary>
        /// <exception cref="Xunit.Sdk.EqualException"></exception>
        [Fact]
        public void ReadStruct()
        {
            TestStruct readStruct = TestFileReader1.ReadStruct<TestStruct>();

            Assert.Equal(134u, readStruct.A);
            Assert.Equal(116u, readStruct.B);
        }
    }
}