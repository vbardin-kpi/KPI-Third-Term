using System;
using System.Collections.Generic;

using FusionDms.Core;
using FusionDms.Exceptions;

namespace FusionDms.Internal
{
    /// <summary>
    /// <b> Manipulates with indexes file </b>
    /// </summary>
    internal class IndexProcessor
    {
        internal Index<TKey> CreateIndex<TKey>(TKey key, List<string> lines)
        {
            return new Index<TKey>(key, GetRecordsAmount(lines) + 1);
        }

        internal List<Index<TKey>> GetIndexes<TKey>(List<string> lines)
        {
            using var indexesIterator = lines.GetEnumerator();

            var indexes = new List<Index<TKey>>();
            var indexSectionBeginMarkerFound = false;
            while (indexesIterator.MoveNext())
            {
                var line = indexesIterator.Current;

                if (line is FusionConstants.IndexAreaBeginMarker)
                {
                    indexSectionBeginMarkerFound = true;
                    continue;
                }
                if (line is FusionConstants.IndexAreaEndMarker) break;

                if (indexSectionBeginMarkerFound)
                {
                    indexes.Add(Index<TKey>.FromString(line));
                }
            }

            return indexes;
        }

        internal Index<TKey> GetIndexByKey<TKey>(List<string> lines, TKey key) where TKey : IComparable
        {
            try
            {
                var indexes = GetIndexes<TKey>(lines);
                return BinarySearch(indexes, key);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new RecordNotFoundException("Record with key " + key + " wasn't found!");
            }
        }

        private Index<TKey> BinarySearch<TKey>(
            IReadOnlyList<Index<TKey>> indexes,
            TKey value) where TKey : IComparable
        {
            var lo = 0;
            var hi = indexes.Count - 1;
            while (lo <= hi)
            {
                var i = lo + ((hi - lo) >> 1);
                var order = indexes[i].Key.CompareTo(value);

                switch (order)
                {
                    case 0:
                        return indexes[i];
                    case < 0:
                        lo = i + 1;
                        break;
                    default:
                        hi = i - 1;
                        break;
                }
            }

            return indexes[~lo];
        }

        private int GetRecordsAmount(List<string> lines)
        {
            using var recordsIterator = lines.GetEnumerator();

            var records = 0;
            var dataBlockStarted = false;

            while (recordsIterator.MoveNext())
            {
                var line = recordsIterator.Current;
                if (line is FusionConstants.DataAreaBeginMarker)
                {
                    dataBlockStarted = true;
                    continue;
                }

                if (dataBlockStarted) records++;
            }

            return records;
        }
    }
}