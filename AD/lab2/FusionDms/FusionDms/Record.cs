using System;
using System.Text;

using FusionDms.Core;

using Newtonsoft.Json;

namespace FusionDms
{
    public class Record<TKey, TVal> where TKey : IComparable
    {
        public TKey Key { get; set; }
        public TVal Value { get; set; }

        public Record(TKey key, TVal val)
        {
            Key = key;
            Value = val;
        }

        internal static Record<TKey, TVal> ToRecord(DataPiece<TKey> dataPiece)
        {
            return new Record<TKey, TVal>(
                dataPiece.Key,
                JsonConvert.DeserializeObject<TVal>(Encoding.UTF8.GetString(dataPiece.Value)));
        }
    }
}