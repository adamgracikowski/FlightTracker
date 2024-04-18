using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public class Position
    {
        public static readonly Single Unknown = 0;
        public Single Longitude { get; set; }
        public Single Latitude { get; set; }
        public Single Amsl { get; set; }
        public Position(Single longitude, Single latitude, Single amsl)
        {
            Update(longitude, latitude, amsl);
        }
        public Position Copy()
        {
            return new Position(Longitude, Latitude, Amsl);
        }
        public void Update(Single longitude, Single latitude, Single? amsl = null)
        {
            if (!IsValidLongitude(longitude) || !IsValidLatitude(latitude))
                throw new InvalidOperationException();

            Longitude = longitude;
            Latitude = latitude;
            Amsl = amsl ?? Amsl;
        }

        private static bool IsValidLongitude(double longitude)
        {
            return _minLongitude <= longitude && longitude <= _maxLongitude;
        }
        private static bool IsValidLatitude(double latitude)
        {
            return _minLatitude <= latitude && latitude <= _maxLatitude;
        }

        private static readonly double _maxLongitude = 180;
        private static readonly double _minLongitude = -180;
        private static readonly double _maxLatitude = 85;
        private static readonly double _minLatitude = -85;
    }
}
