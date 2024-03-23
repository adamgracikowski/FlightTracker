using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Tools
{
    public static class TimeConverter
    {
        /// <summary>
        /// Converts a number of miliseconds since EPOCH to formatted string.
        /// </summary>
        /// <param name="ms">Number of miliseconds since EPOCH.</param>
        /// <returns>String in the following format: HH:mm</returns>
        public static string SinceEpochUtcToString(Int64 ms)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime.ToString("HH:mm");
        }
        public static DateTime SinceEpochUtcToDateTime(Int64 ms)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime;
        }
        public static DateTime ParseExact(string time)
        {
            return DateTime.ParseExact(time, "HH:mm", null);
        }
    }
}
