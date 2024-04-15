using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.DataSource.Tcp;
using ProjOb_24L_01180781.Exceptions;
using ProjOb_24L_01180781.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Database
{
    public static class AviationDatabase
    {
        public static ConcurrentDictionary<UInt64, IAviationItem> AllItems { get; private set; } = [];
        public static DatabaseTable5<Airport> AirportTable { get; private set; }
            = new(TcpAcronyms.Airport, "Airport");
        public static DatabaseTable5<Cargo> CargoTable { get; private set; }
            = new(TcpAcronyms.Cargo, "Cargo");
        public static DatabaseTable5<CargoPlane> CargoPlaneTable { get; private set; }
            = new(TcpAcronyms.CargoPlane, "CargoPlane");
        public static DatabaseTable5<Crew> CrewTable { get; private set; }
            = new(TcpAcronyms.Crew, "Crew");
        public static DatabaseTable5<Flight> FlightTable { get; private set; }
            = new(TcpAcronyms.Flight, "Flight");
        public static DatabaseTable5<Passenger> PassengerTable { get; private set; }
            = new(TcpAcronyms.Passenger, "Passenger");
        public static DatabaseTable5<PassengerPlane> PassengerPlaneTable { get; private set; }
            = new(TcpAcronyms.PassengerPlane, "PassengerPlane");

        public static void Synchronize()
        {
            lock (_cacheLock)
            {
                foreach (var entity in _cache)
                    AllItems.TryAdd(entity.Id, entity);

                AirportTable.AddRange(_cache);
                CargoTable.AddRange(_cache);
                CargoPlaneTable.AddRange(_cache);
                CrewTable.AddRange(_cache);
                FlightTable.AddRange(_cache);
                PassengerTable.AddRange(_cache);
                PassengerPlaneTable.AddRange(_cache);

                _cache.Clear();
            }
        }
        public static void Add(IAviationItem item)
        {
            lock (_cacheLock)
                _cache.Add(item);
        }
        public static void AddRange(IEnumerable<IAviationItem> items)
        {
            lock (_cacheLock)
                _cache.AddRange(items);
        }
        public static List<IAviationItem> CopyState()
        {
            return AllItems.Select(item =>
            {
                lock (item.Value.Lock)
                    return item.Value.Copy();
            }).ToList();
        }
        public static List<IAviationItem> CopyReportable()
        {
            var reportable = new List<IAviationItem>();
            AirportTable.CopyTo(reportable);
            CargoPlaneTable.CopyTo(reportable);
            PassengerPlaneTable.CopyTo(reportable);
            return reportable;
        }
        public static IAviationItem? Find(UInt64 id)
        {
            if (AllItems.TryGetValue(id, out var item) && item is not null)
            {
                return item;
            }
            else return null;
        }

        private static List<IAviationItem> _cache { get; set; } = [];
        private static readonly object _cacheLock = new();
    }
}
