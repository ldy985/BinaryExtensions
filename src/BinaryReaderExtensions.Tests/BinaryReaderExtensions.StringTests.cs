using System;
using System.IO;
using System.Text;
using Xunit;

namespace ldy985.BinaryReaderExtensions.Tests;

public partial class BinaryReaderExtensionsTests
{
    /// <summary>ReadStringNullTerminated</summary>
    /// <exception cref="IOException"></exception>
    /// <exception cref="Xunit.Sdk.EqualException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    [Fact]
    public void ReadStringNullTerminated()
    {
        TestFileReader1.SetPosition(24);
        long indexOf = TestFileReader1.IndexOf(0x00);

        string s = TestFileReader1.ReadFixedString((int)(indexOf - TestFileReader1.GetPosition()), Encoding.ASCII);
        Assert.Equal("ANDROI~1", s);
    }

    /// <summary>
    /// ReadString
    /// </summary>
    /// <exception cref="IOException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="Xunit.Sdk.EqualException"></exception>
    [Fact]
    public void ReadString()
    {
        TestFileReader1.SetPosition(116);
        string s = TestFileReader1.ReadFixedString(14, Encoding.Unicode);
        Assert.Equal("android", s);
    }
}