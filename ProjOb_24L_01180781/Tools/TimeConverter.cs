using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Tools
{
    public static class TimeConverter
    {
        public static readonly string Format = "HH:mm";

        /// <summary>
        /// Converts a number of miliseconds since EPOCH to formatted string.
        /// </summary>
        /// <param name="ms">Number of miliseconds since EPOCH.</param>
        /// <returns>String in the following format: HH:mm</returns>
        public static string SinceEpochUtcToString(Int64 ms)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime.ToString(Format);
        }
        public static DateTime SinceEpochUtcToDateTime(Int64 ms)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime;
        }
        public static DateTime ParseExact(string time)
        {
            return DateTime.ParseExact(time, Format, null);
        }
        public static string FromDateTimeToFormatString(DateTime dateTime)
        {
            return dateTime.ToString(Format);
        }
    }
}
