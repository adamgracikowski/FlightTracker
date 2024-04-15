using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.GUI
{
    public class FlightDetails : FlightGUI
    {
        public Flight Flight { get; private set; }
        public Airport Origin { get; private set; }
        public Airport Target { get; private set; }
        public new WorldPosition WorldPosition
        {
            get
            {
                lock (Flight.Lock)
                {
                    _worldPosition.Longitude = Flight.Position.Longitude;
                    _worldPosition.Latitude = Flight.Position.Latitude;
                }
                return _worldPosition;
            }
        }
        public new double MapCoordRotation { get; private set; }
        public new ulong ID
        {
            get
            {
                ulong id;
                lock (Flight.Lock)
                    id = Flight.Id;
                return id;
            }
        }

        public FlightDetails(Flight flight, Airport origin, Airport target)
            : base()
        {
            Flight = flight;
            Origin = origin;
            Target = target;
            MapCoordRotation = MapCalculator.CalculateRotation(Origin.Position, Target.Position);
        }
        public void UpdateFlightLocation()
        {
            Position origin, target;
            DateTime takeOffDateTime, landingDateTime;

            lock (Origin.Lock)
                origin = Origin.Position.Copy();
            lock (Target.Lock)
                target = Target.Position.Copy();
            lock (Flight.Lock)
            {
                takeOffDateTime = Flight.TakeOffDateTime;
                landingDateTime = Flight.LandingDateTime;
            }

            var (longitude, latitude) = MapCalculator.CalculateLocation(
                Flight.TakeOffDateTime, Flight.LandingDateTime, Origin.Position, Target.Position);

            lock (Flight.Lock)
            {
                Flight.Position.Longitude = (Single)longitude;
                Flight.Position.Latitude = (Single)latitude;
            }
        }
        public void UpdateFlightRotation()
        {
            Position origin, target;
            lock (Origin.Lock)
                origin = Origin.Position.Copy();
            lock (Target.Lock)
                target = Target.Position.Copy();

            MapCoordRotation = MapCalculator.CalculateRotation(origin, target);
        }

        private WorldPosition _worldPosition;
    }
}
