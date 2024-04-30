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

            SetInitialPosition();
            UpdateFlightRotation();
        }
        private void SetInitialPosition()
        {
            Position origin, target;
            DateTime? departure, arrival;

            lock (Origin.Lock)
                origin = Origin.Position.Copy();
            lock (Target.Lock)
                target = Target.Position.Copy();
            lock (Flight.Lock)
            {
                departure = Flight.TakeOffDateTime;
                arrival = Flight.LandingDateTime;
            }

            var (longitude, latitude) = MapCalculator.CalculatePosition(departure, arrival, origin, target);

            lock (Flight.Lock)
                Flight.UpdatePosition(longitude, latitude);
        }
        public void UpdateFlightPosition()
        {
            Position current, target;
            DateTime? departure, arrival;

            lock (Target.Lock)
                target = Target.Position.Copy();
            lock (Flight.Lock)
            {
                current = Flight.StartingPosition.Copy();
                departure = Flight.StartingDateTime;
                arrival = Flight.LandingDateTime;
            }
            var (longitude, latitude) = MapCalculator.CalculatePosition(departure, arrival, current, target);
            lock (Flight.Lock)
                Flight.Position.Update(longitude, latitude);
        }
        public void UpdateFlightRotation()
        {
            Position current, target;
            lock (Flight.Lock)
                current = Flight.Position.Copy();
            lock (Target.Lock)
                target = Target.Position.Copy();
            MapCoordRotation = MapCalculator.CalculateRotation(current, target);
        }

        private WorldPosition _worldPosition;
    }
}