using System;
using System.IO;

using FusionDms.Core;
using FusionDms.Internal;

namespace FusionDms
{
    /// <summary>
    /// <b>This is an outer interface that allows to manipulate with a database file</b>
    /// </summary>
    public class FusionConnection
    {
        // Valid connection string example: "Db=dbfile.fdb;Access=ReadOnly;"
        private readonly FileProcessor _fileProcessor;

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        public FusionConnection(string connectionString, bool createIfNotExists = false)
        {
            ConnectionInfo connectionInfo;

            try
            {
                connectionInfo = ConnectionStringParser.ParseConnectionString(connectionString);
            }
            catch (FileNotFoundException)
            {
                if (!createIfNotExists) throw;

                File.Create(ConnectionStringParser.GetFilePath(connectionString)).Dispose();
                connectionInfo = ConnectionStringParser.ParseConnectionString(connectionString);
            }

            _fileProcessor = new FileProcessor(connectionInfo);
        }

        public Record<TKey, TVal> ReadRecord<TKey, TVal>(TKey key) where TKey : IComparable
        {
            var dataPiece = _fileProcessor.ReadDataPiece(key);
            return Record<TKey, TVal>.ToRecord(dataPiece);
        }

        public TVal Read<TKey, TVal>(TKey key) where TKey : IComparable
        {
            var dataPiece = _fileProcessor.ReadDataPiece(key);
            return Record<TKey, TVal>.ToRecord(dataPiece).Value;
        }

        public Record<TKey, TVal> Write<TKey, TVal>(TKey key, TVal value) where TKey : IComparable
        {
            _fileProcessor.WriteDataPiece(DataPiece<TKey>.Create(key, value));
            return new Record<TKey, TVal>(key, value);
        }

        public void Update<TKey, TVal>(TKey key, TVal newValue) where TKey : IComparable
        {
            var updatedDataPiece = DataPiece<TKey>.Create(key, newValue);
            _fileProcessor.UpdateDataPiece(updatedDataPiece.Key, updatedDataPiece);
        }

        public void Delete<TKey>(TKey key) where TKey : IComparable
        {
            _fileProcessor.DeleteDataPiece(key);
        }
    }
}