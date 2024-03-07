using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Tools
{
    public static class MsConverter
    {
        /// <summary>
        /// Converts a number of miliseconds since EPOCH to formatted string.
        /// </summary>
        /// <param name="ms">Number of miliseconds since EPOCH.</param>
        /// <returns>String in the following format: HH:mm</returns>
        public static string SinceEpochUtc(Int64 ms)
        {
            var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime;
            var formattedTime = dateTime.ToString("HH:mm");
            return formattedTime;
        }
    }
}
