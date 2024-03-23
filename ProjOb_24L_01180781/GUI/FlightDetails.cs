using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.GUI
{
    public class FlightDetails
    {
        public Flight Flight { get; private set; }
        public Airport Origin { get; private set; }
        public Airport Target { get; private set; }
        public double Rotation { get; private set; }
        public FlightDetails(Flight flight, Airport origin, Airport target)
        {
            Flight = flight;
            Origin = origin;
            Target = target;
            Rotation = MapCalculator.CalculateRotation(Origin.Location, Target.Location);
        }
        public void UpdateFlightLocation()
        {
            var departure = Flight.TakeOffDateTime ??
                TimeConverter.ParseExact(Flight.TakeOffTime);
            var arrival = Flight.LandingDateTime ??
                TimeConverter.ParseExact(Flight.LandingTime);

            var (longitude, latitude) = MapCalculator.CalculateLocation(
                departure, arrival, Origin.Location, Target.Location);

            Flight.Location.Longitude = (Single)longitude;
            Flight.Location.Latitude = (Single)latitude;
        }
    }
}
