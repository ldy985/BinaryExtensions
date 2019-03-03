using System.IO;
using BinaryExtensions.LoggedBinaryReader;

namespace SDK.Extensions.LoggedBinaryReader
{
    public class LoggedBinaryReaderMap
    {
        public uint[] Map { get; }

        public LoggedBinaryReaderMap(long mapLength)
        {
            Map = new uint[mapLength];
        }

        public int Count => Map.Length;

        public uint this[int index]
        {
            get => Map[index];
            set => Map[index] = value;
        }

        public void AddRegion(LoggedBinaryReaderRegion region)
        {
            for (long i = region.Position; i < region.Position + region.Length; i++)
                Map[i]++;
        }

        public void SaveToFile(string filepath)
        {
            using (FileStream fileStream = File.OpenWrite(filepath))
            using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
            {
                foreach (uint u in Map)
                {
                    binaryWriter.Write(u <= 255 ? (byte)u : (byte)255);
                }
            }
        }
    }
}