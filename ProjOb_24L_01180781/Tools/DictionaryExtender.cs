using ProjOb_24L_01180781.AviationItems.Interfaces;

namespace ProjOb_24L_01180781.Tools
{
    public static class DictionaryExtender
    {
        public static (List<KeyValuePair<UInt64, TValue>> True, List<KeyValuePair<UInt64, TValue>> False)
            PartitionBy<UInt64, TValue>(this IDictionary<UInt64, TValue> dictionary, Predicate<KeyValuePair<UInt64, TValue>> predicate)
            where TValue : ILockable
        {
            var kvpTrue = new List<KeyValuePair<UInt64, TValue>>();
            var kvpFalse = new List<KeyValuePair<UInt64, TValue>>();
            foreach (var kvp in dictionary)
            {
                lock (kvp.Value.Lock)
                {
                    if (predicate(kvp)) kvpTrue.Add(kvp);
                    else kvpFalse.Add(kvp);
                }
            }
            return (kvpTrue, kvpFalse);
        }
    }
}
