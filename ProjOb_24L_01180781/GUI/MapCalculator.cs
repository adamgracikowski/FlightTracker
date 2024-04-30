using Mapsui.Projections;
using ProjOb_24L_01180781.AviationItems;

namespace ProjOb_24L_01180781.GUI
{
    public static class MapCalculator
    {
        public static double CalculateRotation(Position origin, Position target)
        {
            var (originX, originY) = SphericalMercator.FromLonLat(origin.Longitude, origin.Latitude);
            var (targetX, targetY) = SphericalMercator.FromLonLat(target.Longitude, target.Latitude);

            var rotation = Math.PI / 2.0 - Math.Atan2(targetY - originY, targetX - originX);
            if (rotation < 0.0) rotation += 2 * Math.PI;

            return rotation;
        }
        public static (double longitude, double latitude) CalculatePosition
            (DateTime? departure, DateTime? arrival,
            Position origin, Position target)
        {
            if (departure is null || arrival is null)
            {
                return (origin.Latitude, origin.Longitude);
            }

            var now = DateTime.UtcNow;
            if (now <= departure)
            {
                return (origin.Longitude, origin.Latitude);
            }
            if (now >= arrival)
            {
                return (target.Longitude, target.Latitude);
            }

            var totalMilliseconds = (arrival - departure)?.TotalMilliseconds ?? 0;
            var elapsedMilliseconds = (now - departure)?.TotalMilliseconds ?? 0;

            var deltaLongitude = target.Longitude - origin.Longitude;
            var deltaLatitude = target.Latitude - origin.Latitude;

            var speedLongitude = deltaLongitude / totalMilliseconds;
            var speedLatitude = deltaLatitude / totalMilliseconds;

            var longitude = origin.Longitude + speedLongitude * elapsedMilliseconds;
            var latitude = origin.Latitude + speedLatitude * elapsedMilliseconds;

            return (longitude, latitude);
        }
    }
}
