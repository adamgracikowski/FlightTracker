using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public struct Location
    {
        public static readonly Single Unknown = 0;
        public Single Longitude { get; set; }
        public Single Latitude { get; set; }
        public Single Amsl { get; set; }
        public Location(Single longitude, Single latitude, Single amsl)
        {
            Longitude = longitude;
            Latitude = latitude;
            Amsl = amsl;
        }
    }
}
