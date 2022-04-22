using System.IO;

namespace ldy985.BinaryReaderExtensions.LoggedReader;

public class LoggedBinaryReaderMap
{
    public LoggedBinaryReaderMap(long mapLength)
    {
        Map = new uint[mapLength];
    }

    public int Count => Map.Length;
    public uint[] Map { get; }

    public uint this[int index]
    {
        get => Map[index];
        set => Map[index] = value;
    }

    public void AddRegion(LoggedBinaryReaderRegion region)
    {
        for (long i = region.Position; i < region.Position + region.Length; i++)
        {
            Map[i]++;
        }
    }

    /// <summary>
    /// SaveToFileMask. Saves to a file that has the same length as the map. With each count clamped to one byte.
    /// </summary>
    /// <param name="filepath"></param>
    /// <exception cref="IOException"></exception>
    /// <exception cref="System.ObjectDisposedException"></exception>
    /// <exception cref="System.UnauthorizedAccessException"></exception>
    public void SaveToFileMask(string filepath)
    {
        using (FileStream fileStream = File.OpenWrite(filepath))
        using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
        {
            foreach (uint u in Map)
                binaryWriter.Write(u <= 255 ? (byte)u : (byte)255);
        }
    }
}