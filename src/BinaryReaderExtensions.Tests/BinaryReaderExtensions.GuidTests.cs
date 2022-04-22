using System;
using Xunit;

namespace ldy985.BinaryReaderExtensions.Tests;

public partial class BinaryReaderExtensionsTests
{
    [Fact]
    public void ReadGuid()
    {
        Guid guid = TestFileReader1.ReadGuid();
        Assert.Equal(guid, new Guid("00740086-001e-4643-5346-180031000000"));
    }
}