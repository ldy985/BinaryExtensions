using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ldy985.BinaryReaderExtensions.LoggedReader

{
    internal sealed class LoggedBinaryReaderJournal : IList<LoggedBinaryReaderRegion>
    {
        internal LoggedBinaryReaderJournal(LoggedBinaryReader reader)
        {
            Reader = reader;
            Regions = new List<LoggedBinaryReaderRegion>();
        }

        private long Position { get; set; } = -1;

        private LoggedBinaryReader Reader { get; }

        private List<LoggedBinaryReaderRegion> Regions { get; }

        /// <summary>BeginLog</summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void BeginLog()
        {
            if (Position != -1)
                throw new InvalidOperationException($"Call {nameof(EndLog)} first.");

            Position = Reader.BaseStream.Position;
        }

        /// <summary>EndLog</summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void EndLog()
        {
            if (Position == -1)
                throw new InvalidOperationException($"Call {nameof(BeginLog)} first.");

            Regions.Add(new LoggedBinaryReaderRegion(Position, Reader.BaseStream.Position - Position));

            Position = -1;
        }

        public IEnumerable<LoggedBinaryReaderRegion> GetOrderedRegions()
        {
            return Regions.OrderBy(s => s.Position);
        }

        #region IList

        public int Count => Regions.Count;

        public bool IsReadOnly => ((ICollection<LoggedBinaryReaderRegion>)Regions).IsReadOnly;

        public LoggedBinaryReaderRegion this[int index]
        {
            get => Regions[index];
            set => Regions[index] = value;
        }

        public void Add(LoggedBinaryReaderRegion item)
        {
            Regions.Add(item);
        }

        public void Clear()
        {
            Regions.Clear();
        }

        public bool Contains(LoggedBinaryReaderRegion item)
        {
            return Regions.Contains(item);
        }

        public void CopyTo(LoggedBinaryReaderRegion[] array, int arrayIndex)
        {
            Regions.CopyTo(array, arrayIndex);
        }

        public IEnumerator<LoggedBinaryReaderRegion> GetEnumerator()
        {
            return Regions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(LoggedBinaryReaderRegion item)
        {
            return Regions.IndexOf(item);
        }

        public void Insert(int index, LoggedBinaryReaderRegion item)
        {
            Regions.Insert(index, item);
        }

        public bool Remove(LoggedBinaryReaderRegion item)
        {
            return Regions.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Regions.RemoveAt(index);
        }

        #endregion IList
    }
}