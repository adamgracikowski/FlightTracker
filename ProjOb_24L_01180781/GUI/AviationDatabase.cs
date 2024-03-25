using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.Exceptions;
using ProjOb_24L_01180781.Tcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.GUI
{
    public static class AviationDatabase
    {
        public static List<IAviationItem> AviationItems { get; private set; } = [];
        public static readonly object AviationItemsLock = new();
        public static List<FlightDetails> FlightDetails { get; private set; } = [];

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
        public static void SyncAviationItems()
        {
            lock (AviationItemsLock)
            {
                var rangeToSync = AviationItems[_startUpdateFrom..];
                SyncAirports(rangeToSync);
                SyncFlights(rangeToSync);
                _startUpdateFrom += rangeToSync.Count;
            }
        }

        private static void SyncAirports(IEnumerable<IAviationItem> rangeToSync)
        {
            var airports = rangeToSync
                .Where(item => item.TcpAcronym == TcpAcronyms.Airport)
                .Cast<Airport>();

            foreach (var airport in airports)
            {
                if (!_airportsDictionary.TryAdd(airport.Id, airport))
                {
                    var message = $"duplicated airport ID ({airport.Id})";
                    throw new TcpFormatException(message);
                }
            }
        }
        private static void SyncFlights(IEnumerable<IAviationItem> rangeToSync)
        {
            var flights = rangeToSync
                .Where(item => item.TcpAcronym == TcpAcronyms.Flight)
                .Cast<Flight>();

            FlightDetails.AddRange(flights.Select(flight =>
                new FlightDetails(flight, FindAirport(flight.OriginId), FindAirport(flight.TargetId))
            ));
        }
        private static Airport FindAirport(UInt64 id)
        {
            if (_airportsDictionary.TryGetValue(id, out var airport) && airport is not null)
            {
                return airport;
            }
            else
            {
                var message = $"unknown airport ID ({id})";
                throw new TcpFormatException(message);
            }
        }

        private static Dictionary<UInt64, Airport> _airportsDictionary = [];
        private static int _startUpdateFrom = 0;
    }
}
