using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace ldy985.BinaryReaderExtensions.LoggedReader
{
    [PublicAPI]

    /// <summary>
    ///     A <see cref="T:System.IO.BinaryReader" /> that logs the regions it reads.
    /// </summary>
    public sealed class LoggedBinaryReader : BinaryReader
    {
        /// <summary>Initializes a new instance of <see cref="LoggedBinaryReader" />.</summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="encoding">The character encoding to use, <code>null</code> for <see cref="Encoding.Default" />.</param>
        /// <param name="leaveOpen">
        /// <code>true</code> to leave the stream open after the <see cref="System.IO.BinaryReader" />
        /// object is disposed; otherwise, <code>false</code>.
        /// </param>
        public LoggedBinaryReader(Stream stream, Encoding encoding = null, bool leaveOpen = true) : base(stream, encoding ?? Encoding.Default, leaveOpen)
        {
            Journal = new LoggedBinaryReaderJournal(this);
        }

        private int Group { get; set; } = -1;

        private LoggedBinaryReaderJournal Journal { get; }

        /// <summary>
        /// Scope
        /// </summary>
        /// <exception cref="InvalidOperationException">Get. Ignore.</exception>
        private IDisposable Scope => new LoggedBinaryReaderScope(Journal);

        /// <summary>Begins a block of regions to group (see Remarks).</summary>
        /// <remarks>Each region grouping must be disposed of when done.</remarks>
        /// <exception cref="InvalidOperationException"></exception>
        public IDisposable BeginGroup()
        {
            return new LoggedBinaryReaderGroup(this);
        }

        /// <summary>Removes all regions that have been read.</summary>
        public void Clear()
        {
            Journal.Clear();
        }

        public LoggedBinaryReaderMap GetReadCountMap()
        {
            IEnumerable<LoggedBinaryReaderRegion> readRegions = GetReadRegions(false);
            LoggedBinaryReaderMap readCountMap = new LoggedBinaryReaderMap(BaseStream.Length);

            foreach (LoggedBinaryReaderRegion loggedBinaryReaderRegion in readRegions)
            {
                readCountMap.AddRegion(loggedBinaryReaderRegion);
            }

            return readCountMap;
        }

        /// <summary>Gets the regions that have been read.</summary>
        /// <param name="merge">Whether to merge overlapping regions.</param>
        /// <returns>The regions read.</returns>
        public IEnumerable<LoggedBinaryReaderRegion> GetReadRegions(bool merge = true)
        {
            IEnumerable<LoggedBinaryReaderRegion> regions = Journal.GetOrderedRegions();
            return merge ? Merge(regions) : regions;
        }

        /// <summary>Gets the size that have been read.</summary>
        /// <param name="merge">Whether to merge overlapping regions.</param>
        /// <returns>The size read.</returns>
        public ulong GetReadSize(bool merge = true)
        {
            return (ulong)GetReadRegions(merge).Sum(region => region.Length);
        }

        /// <summary>Gets the regions that haven't been read.</summary>
        /// <returns>The unread regions.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public IEnumerable<LoggedBinaryReaderRegion> GetUnreadRegions()
        {
            IEnumerable<LoggedBinaryReaderRegion> regions = Journal.GetOrderedRegions();

            using (IEnumerator<LoggedBinaryReaderRegion> e = regions.GetEnumerator())
            {
                // nothing at all
                if (!e.MoveNext())
                {
                    yield return new LoggedBinaryReaderRegion(0, BaseStream.Length);
                    yield break;
                }

                LoggedBinaryReaderRegion first = e.Current;

                // beginning of file
                if (first.Position > 0)
                    yield return new LoggedBinaryReaderRegion(0, first.Position);

                // middle of file
                while (e.MoveNext())
                {
                    LoggedBinaryReaderRegion second = e.Current;
                    long secondPos = first.Position + first.Length;
                    long secondLen = second.Position - secondPos;

                    if (secondLen > 0)
                        yield return new LoggedBinaryReaderRegion(secondPos, secondLen);

                    first = second;
                }

                // end of file
                long thirdPos = first.Position + first.Length;
                long thirdLen = BaseStream.Length - thirdPos;
                if (thirdPos < BaseStream.Length)
                    yield return new LoggedBinaryReaderRegion(thirdPos, thirdLen);
            }
        }

        /// <inheritdoc />
        public override int Read()
        {
            using (Scope)
                return base.Read();
        }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int index, int count)
        {
            using (Scope)
                return base.Read(buffer, index, count);
        }

        /// <inheritdoc />
        public override int Read(char[] buffer, int index, int count)
        {
            using (Scope)
                return base.Read(buffer, index, count);
        }

        /// <inheritdoc />
        public override bool ReadBoolean()
        {
            using (Scope)
                return base.ReadBoolean();
        }

        /// <inheritdoc />
        public override byte ReadByte()
        {
            using (Scope)
                return base.ReadByte();
        }

        /// <inheritdoc />
        public override byte[] ReadBytes(int count)
        {
            using (Scope)
                return base.ReadBytes(count);
        }

        /// <inheritdoc />
        public override char ReadChar()
        {
            using (Scope)
                return base.ReadChar();
        }

        /// <inheritdoc />
        public override char[] ReadChars(int count)
        {
            using (Scope)
                return base.ReadChars(count);
        }

        /// <inheritdoc />
        public override decimal ReadDecimal()
        {
            using (Scope)
                return base.ReadDecimal();
        }

        /// <inheritdoc />
        public override double ReadDouble()
        {
            using (Scope)
                return base.ReadDouble();
        }

        /// <inheritdoc />
        public override short ReadInt16()
        {
            using (Scope)
                return base.ReadInt16();
        }

        /// <inheritdoc />
        public override int ReadInt32()
        {
            using (Scope)
                return base.ReadInt32();
        }

        /// <inheritdoc />
        public override long ReadInt64()
        {
            using (Scope)
                return base.ReadInt64();
        }

        /// <inheritdoc />
        public override sbyte ReadSByte()
        {
            using (Scope)
                return base.ReadSByte();
        }

        /// <inheritdoc />
        public override float ReadSingle()
        {
            using (Scope)
                return base.ReadSingle();
        }

        /// <inheritdoc />
        public override string ReadString()
        {
            using (Scope)
                return base.ReadString();
        }

        /// <inheritdoc />
        public override ushort ReadUInt16()
        {
            using (Scope)
                return base.ReadUInt16();
        }

        /// <inheritdoc />
        public override uint ReadUInt32()
        {
            using (Scope)
                return base.ReadUInt32();
        }

        /// <inheritdoc />
        public override ulong ReadUInt64()
        {
            using (Scope)
                return base.ReadUInt64();
        }

        /// <summary>BeginGroupInternal</summary>
        /// <exception cref="InvalidOperationException"></exception>
        internal void BeginGroupInternal()
        {
            if (Group != -1)
                throw new InvalidOperationException($"A previous call to {nameof(BeginGroup)} has not been disposed.");

            Group = Journal.Count;
        }

        /// <summary>EndGroupInternal</summary>
        /// <exception cref="InvalidOperationException"></exception>
        internal void EndGroupInternal()
        {
            if (Group == -1)
                throw new InvalidOperationException($"A previous call to {nameof(BeginGroup)} has not been disposed.");

            LoggedBinaryReaderRegion[] regions1 = Journal.Skip(Group).ToArray();
            LoggedBinaryReaderRegion[] regions2 = Merge(regions1).ToArray();

            if (regions2.Length > 1)
                throw new NotSupportedException("Region grouping only supports consecutive regions.");

            int count = Journal.Count - Group;

            for (int i = 0; i < count; i++)
            {
                Journal.RemoveAt(Journal.Count - 1);
            }

            LoggedBinaryReaderRegion region = regions2.First();

            Journal.Add(region);

            Group = -1;
        }

        /// <summary>
        /// Merge
        /// </summary>
        /// <param name="regions"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private IEnumerable<LoggedBinaryReaderRegion> Merge(IEnumerable<LoggedBinaryReaderRegion> regions)
        {
            using (IEnumerator<LoggedBinaryReaderRegion> e = regions.GetEnumerator())
            {
                if (!e.MoveNext())
                    yield break;

                long origin = e.Current.Position;
                long ending = e.Current.Position + e.Current.Length;

                while (e.MoveNext())
                {
                    LoggedBinaryReaderRegion current = e.Current;
                    long currentPos = current.Position;
                    long currentLen = current.Length;

                    if (currentPos > ending)
                    {
                        yield return new LoggedBinaryReaderRegion(origin, ending - origin);
                        origin = currentPos;
                        ending = currentPos + currentLen;
                    }
                    else
                        ending = Math.Max(currentPos + currentLen, ending);
                }

                yield return new LoggedBinaryReaderRegion(origin, ending - origin);
            }
        }
    }
}