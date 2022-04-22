using System;

namespace ldy985.BinaryReaderExtensions.LoggedReader;

internal sealed class LoggedBinaryReaderGroup : IDisposable
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="reader"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public LoggedBinaryReaderGroup(LoggedBinaryReader reader)
    {
        Reader = reader;
        reader.BeginGroupInternal();
    }

    private LoggedBinaryReader Reader { get; }

    /// <summary>
    /// Dispose
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public void Dispose()
    {
        Reader.EndGroupInternal();
    }
}