using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Tools
{
    public static class MsConverter
    {
        public static string SinceEpochUtc(Int64 ms)
        {
            var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime;
            var formattedTime = dateTime.ToString("HH:mm");
            return formattedTime;
        }
    }
}
