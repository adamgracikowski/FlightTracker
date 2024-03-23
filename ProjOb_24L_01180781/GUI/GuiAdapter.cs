using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.GUI
{
    public static class GuiAdapter
    {
        public static FlightGUI FlightDetailsToFlightGui(FlightDetails flightDetail)
        {
            return new FlightGUI()
            {
                ID = flightDetail.Flight.Id,
                MapCoordRotation = flightDetail.Rotation,
                WorldPosition = new WorldPosition(
                    flightDetail.Flight.Location.Latitude,
                    flightDetail.Flight.Location.Longitude)
            };
        }
    }
}
