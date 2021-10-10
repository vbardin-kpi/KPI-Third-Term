using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FusionDms.Core;
using FusionDms.Exceptions;

namespace FusionDms.Internal
{
    internal class FileProcessor
    {
        private readonly ConnectionInfo _connectionInfo;
        private readonly IndexProcessor _indexProcessor;

        internal FileProcessor(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
            _indexProcessor = new IndexProcessor();
        }

        internal void WriteDataPiece<TKey>(DataPiece<TKey> dataPiece) where TKey : IComparable
        {
            List<Index<TKey>> indexes;
            List<DataPiece<TKey>> dataSegment;

            using (var fs = GetFileStream())
            {
                if (!fs.CanWrite)
                {
                    throw new InvalidOperationException("Database connection opened in read-only mode!");
                }

                var lines = ReadAllLines(fs);

                var newDataPieceIndex = _indexProcessor.CreateIndex(dataPiece.Key, lines);

                indexes = _indexProcessor.GetIndexes<TKey>(lines);
                indexes.Add(newDataPieceIndex);
                indexes = indexes.OrderBy(x => x.Key).ToList();

                dataSegment = GetDataSegment<TKey>(lines);
                dataSegment.Add(dataPiece);
            }

            RewriteFile(indexes, dataSegment);
        }

        internal DataPiece<TKey> ReadDataPiece<TKey>(TKey key) where TKey : IComparable
        {
            using var fs = GetFileStream();
            var lines = ReadAllLines(fs);

            var index = _indexProcessor.GetIndexByKey(lines, key);

            return GetDataPieceByLineNumber<TKey>(index.Payload);
        }

        internal DataPiece<TKey> UpdateDataPiece<TKey>(TKey key, DataPiece<TKey> newValue)
            where TKey : IComparable
        {
            if (key.CompareTo(newValue.Key) != 0)
            {
                throw new InvalidOperationException("Record and key aren't consistent!");
            }

            List<string> lines;

            using (var fs = GetFileStream())
            {
                lines = ReadAllLines(fs);
            }

            RewriteDataPiece(lines, newValue);

            return newValue;
        }

        internal void DeleteDataPiece<TKey>(TKey key) where TKey : IComparable
        {
            List<Index<TKey>> indexes;
            List<DataPiece<TKey>> dataSegment;

            using (var fs = GetFileStream())
            {
                var lines = ReadAllLines(fs);
                indexes = _indexProcessor.GetIndexes<TKey>(lines);

                dataSegment = GetDataSegment<TKey>(lines);

                indexes.RemoveAll(x => key.CompareTo(x.Key) == 0);
                dataSegment.RemoveAll(x => key.CompareTo(x.Key) == 0);
            }

            RewriteFile(indexes, dataSegment);
        }

        private List<string> ReadAllLines(FileStream fs)
        {
            var lines = new List<string>();

            using var reader = new StreamReader(fs);
            while (!reader.EndOfStream)
            {
                lines.Add(reader.ReadLine());
            }

            return lines;
        }

        private void RewriteDataPiece<TKey>(
            List<string> lines,
            DataPiece<TKey> newValue) where TKey : IComparable
        {
            var dataSegment = GetDataSegment<TKey>(lines);

            var existedDataPiece = dataSegment
                .FirstOrDefault(x => x.Key.CompareTo(newValue.Key) == 0);

            if (existedDataPiece is null)
            {
                throw new RecordNotFoundException(
                    "Record with key " + newValue.Key + " wasn't found!");
            }

            existedDataPiece.Value = newValue.Value;

            var indexes = _indexProcessor.GetIndexes<TKey>(lines);

            RewriteFile(indexes, dataSegment);
        }

        private void RewriteFile<TKey>(
            IEnumerable<Index<TKey>> indexes,
            IEnumerable<DataPiece<TKey>> dataSegment) where TKey : IComparable
        {
            var res = new List<string> { FusionConstants.IndexAreaBeginMarker };
            res.AddRange(indexes.Select(x => x.ToString()));
            res.Add(FusionConstants.IndexAreaEndMarker);
            res.Add(FusionConstants.DataAreaBeginMarker);
            res.AddRange(dataSegment.Select(x => x.ToString()));

            using var writer = new StreamWriter(_connectionInfo.FilePath, append: false);
            foreach (var line in res)
            {
                writer.WriteLine(line);
            }

            writer.Flush();
        }

        private List<DataPiece<TKey>> GetDataSegment<TKey>(List<string> lines) where TKey : IComparable
        {
            var dataSegment = new List<DataPiece<TKey>>();
            var isDataSegment = false;
            using var dataIterator = lines.GetEnumerator();
            while (dataIterator.MoveNext())
            {
                var line = dataIterator.Current;

                if (line is FusionConstants.DataAreaBeginMarker) isDataSegment = true;
                if (IsMarkLine(line)) continue;

                if (isDataSegment)
                {
                    dataSegment.Add(DataPiece<TKey>.ToDataPiece(line));
                }
            }

            return dataSegment;
        }

        private DataPiece<TKey> GetDataPieceByLineNumber<TKey>(int lineNumber) where TKey : IComparable
        {
            using var reader = new StreamReader(GetFileStream());

            var isDataSegment = false;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (line is FusionConstants.DataAreaBeginMarker) isDataSegment = true;
                if (IsMarkLine(line)) continue;
                if (!isDataSegment) continue;

                for (var i = 0; i <= lineNumber; i++)
                {
                    if (i == lineNumber) return DataPiece<TKey>.ToDataPiece(line);

                    line = reader.ReadLine();
                }
            }

            throw new RecordNotFoundException("An error occured. Record at line " + lineNumber + " wasn't found.");
        }

        private FileStream GetFileStream() =>
            _connectionInfo.AccessMode switch
            {
                FusionAccessMode.ReadWrite => File.Open(_connectionInfo.FilePath, FileMode.Open),
                FusionAccessMode.ReadOnly => File.OpenRead(_connectionInfo.FilePath),
                _ => throw new ArgumentOutOfRangeException(nameof(_connectionInfo.AccessMode), "Invalid access mode.")
            };

        private static bool IsMarkLine(string str) =>
            str is FusionConstants.DataAreaBeginMarker or
                FusionConstants.IndexAreaBeginMarker or
                FusionConstants.IndexAreaEndMarker
            || str.Contains(FusionConstants.IndexAreaEndMarker);
    }
}