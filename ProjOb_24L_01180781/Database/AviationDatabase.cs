using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.DataSource.Tcp;
using ProjOb_24L_01180781.Exceptions;
using ProjOb_24L_01180781.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Database
{
    public static class AviationDatabase
    {
        public static void Add(IAviationItem item)
        {
            lock (AviationItemsLock)
            {
                AviationItems.Add(item);
            }
        }
        public static void AddRange(IEnumerable<IAviationItem> items)
        {
            lock (AviationItemsLock)
            {
                AviationItems.AddRange(items);
            }
        }
        public static void Synchronize()
        {
            lock (AviationItemsLock)
            {
                AirportTable.AddRange(AviationItems);
                CargoTable.AddRange(AviationItems);
                CargoPlaneTable.AddRange(AviationItems);
                CrewTable.AddRange(AviationItems);
                FlightTable.AddRange(AviationItems);
                PassengerTable.AddRange(AviationItems);
                PassengerPlaneTable.AddRange(AviationItems);
                AviationItems.Clear();
            }
        }
        public static List<IAviationItem> CopyState()
        {
            var state = new List<IAviationItem>();
            AirportTable.CopyTo(state);
            CargoTable.CopyTo(state);
            CargoPlaneTable.CopyTo(state);
            CrewTable.CopyTo(state);
            FlightTable.CopyTo(state);
            PassengerTable.CopyTo(state);
            PassengerPlaneTable.CopyTo(state);
            return state;
        }
        public static List<IAviationItem> CopyReportable()
        {
            var reportable = new List<IAviationItem>();
            AirportTable.CopyTo(reportable);
            CargoPlaneTable.CopyTo(reportable);
            PassengerPlaneTable.CopyTo(reportable);
            return reportable;
        }

        public static List<IAviationItem> AviationItems { get; private set; } = new();
        public static readonly object AviationItemsLock = new();

        public static DatabaseTable<Airport> AirportTable { get; private set; }
            = new(TcpAcronyms.Airport, "Airport");
        public static DatabaseTable<Cargo> CargoTable { get; private set; }
            = new(TcpAcronyms.Cargo, "Cargo");
        public static DatabaseTable<CargoPlane> CargoPlaneTable { get; private set; }
            = new(TcpAcronyms.CargoPlane, "CargoPlane");
        public static DatabaseTable<Crew> CrewTable { get; private set; }
            = new(TcpAcronyms.Crew, "Crew");
        public static DatabaseTable<Flight> FlightTable { get; private set; }
            = new(TcpAcronyms.Flight, "Flight");
        public static DatabaseTable<Passenger> PassengerTable { get; private set; }
            = new(TcpAcronyms.Passenger, "Passenger");
        public static DatabaseTable<PassengerPlane> PassengerPlaneTable { get; private set; }
            = new(TcpAcronyms.PassengerPlane, "PassengerPlane");
    }
}
