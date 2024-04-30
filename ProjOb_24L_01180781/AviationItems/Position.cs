namespace ProjOb_24L_01180781.AviationItems
{
    public class Position
    {
        public static readonly double Unknown = 0;
        public double Longitude;
        public double Latitude;
        public double Amsl;
        public Position(double? longitude = null, double? latitude = null, double? amsl = null)
        {
            Update(longitude, latitude, amsl);
        }
        public Position()
        {
            Update(Unknown, Unknown, Unknown);
        }
        public Position Copy()
        {
            return new Position(Longitude, Latitude, Amsl);
        }
        public void Update(double? longitude = null, double? latitude = null, double? amsl = null)
        {
            Longitude = longitude ?? Longitude;
            Latitude = latitude ?? Latitude;
            Amsl = amsl ?? Amsl;
        }

        public static bool IsValidLongitude(double longitude)
        {
            return _minLongitude <= longitude && longitude <= _maxLongitude;
        }
        public static bool IsValidLatitude(double latitude)
        {
            return _minLatitude <= latitude && latitude <= _maxLatitude;
        }

        private static readonly double _maxLongitude = 180;
        private static readonly double _minLongitude = -180;
        private static readonly double _maxLatitude = 85;
        private static readonly double _minLatitude = -85;
    }
}
