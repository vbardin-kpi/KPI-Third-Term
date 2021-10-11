using System;
using System.Text;

using Newtonsoft.Json;

namespace FusionDms.Core
{
    internal class DataPiece<TKey> where TKey : IComparable
    {
        public TKey Key { get; set; }
        public byte[] Value { get; set; }

        public static DataPiece<TKey> Create<TVal>(TKey key, TVal value)
        {
            return new DataPiece<TKey>
            {
                Key = key,
                Value = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)),
            };
        }

        public static DataPiece<TKey> ToDataPiece(string str)
        {
            return JsonConvert.DeserializeObject<DataPiece<TKey>>(str);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
    }
}