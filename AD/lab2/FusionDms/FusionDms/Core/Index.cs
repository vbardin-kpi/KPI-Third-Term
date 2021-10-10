using Newtonsoft.Json;

namespace FusionDms.Core
{
    internal class Index<TKey>
    {
        public TKey Key { get; set; }
        public int Payload { get; set; }

        public Index()
        {
        }

        public Index(TKey key, int payload)
        {
            Key = key;
            Payload = payload;
        }

        public static Index<TKey> FromString(string indexStr)
        {
            return JsonConvert.DeserializeObject<Index<TKey>>(indexStr);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
    }
}