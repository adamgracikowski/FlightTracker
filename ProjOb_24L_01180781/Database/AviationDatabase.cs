using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.DataSource.Tcp;
using ProjOb_24L_01180781.Tools;
using System.Collections.Concurrent;

namespace ProjOb_24L_01180781.Database
{
    public static class AviationDatabase
    {
        public static ConcurrentDictionary<UInt64, IAviationItem> AllItems { get; private set; } = [];
        public static ConcurrentDictionary<string, DatabaseTable<IAviationItem>> Tables { get; private set; } = [];
        public static Dictionary<string, string> TableNames { get; private set; } = new(new KeyComparer());
        static AviationDatabase()
        {
            BuildTables();
            BuildTableNames();
            SubscribeToOnRemoved();
        }

        public static void Synchronize()
        {
            lock (_cacheLock)
            {
                var added = new List<IAviationItem>();
                foreach (var entity in _cache)
                {
                    if (!AllItems.TryAdd(entity.Id, entity))
                    {
                        Console.WriteLine($"Database: Element with ID = {entity.Id} already exists.");
                    }
                    else added.Add(entity);
                }

                Tables[TcpAcronyms.Airport].AddRange(added);
                Tables[TcpAcronyms.Cargo].AddRange(added);
                Tables[TcpAcronyms.CargoPlane].AddRange(added);
                Tables[TcpAcronyms.Crew].AddRange(added);
                Tables[TcpAcronyms.Flight].AddRange(added);
                Tables[TcpAcronyms.Passenger].AddRange(added);
                Tables[TcpAcronyms.PassengerPlane].AddRange(added);

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
            Tables[TcpAcronyms.Airport].CopyTo(reportable);
            Tables[TcpAcronyms.CargoPlane].CopyTo(reportable);
            Tables[TcpAcronyms.PassengerPlane].CopyTo(reportable);
            return reportable;
        }
        public static IAviationItem? Find(UInt64? id)
        {
            if (id is null) return null;
            if (AllItems.TryGetValue(id.Value, out var item) && item is not null)
            {
                return item;
            }
            else return null;
        }

        private static void BuildTables()
        {
            Tables.TryAdd(TcpAcronyms.Airport, new(TcpAcronyms.Airport, AviationName.Airport));
            Tables.TryAdd(TcpAcronyms.Cargo, new(TcpAcronyms.Cargo, AviationName.Cargo));
            Tables.TryAdd(TcpAcronyms.CargoPlane, new(TcpAcronyms.CargoPlane, AviationName.CargoPlane));
            Tables.TryAdd(TcpAcronyms.Crew, new(TcpAcronyms.Crew, AviationName.Crew));
            Tables.TryAdd(TcpAcronyms.Flight, new(TcpAcronyms.Flight, AviationName.Flight));
            Tables.TryAdd(TcpAcronyms.Passenger, new(TcpAcronyms.Passenger, AviationName.Passenger));
            Tables.TryAdd(TcpAcronyms.PassengerPlane, new(TcpAcronyms.PassengerPlane, AviationName.PassengerPlane));
        }
        private static void BuildTableNames()
        {
            TableNames = new(new KeyComparer())
            {
                { AviationName.Airport,        TcpAcronyms.Airport },
                { AviationName.Cargo,          TcpAcronyms.Cargo },
                { AviationName.CargoPlane,     TcpAcronyms.CargoPlane },
                { AviationName.Crew,           TcpAcronyms.Crew },
                { AviationName.Flight,         TcpAcronyms.Flight },
                { AviationName.Passenger,      TcpAcronyms.Passenger },
                { AviationName.PassengerPlane, TcpAcronyms.PassengerPlane }
            };
        }

        private static void SubscribeToOnRemoved()
        {
            // Flight:
            Tables[TcpAcronyms.Flight].ElementRemoved +=
                (sender, args) =>
                {
                    RemoveFromAllItems(args);
                };

            // Airport:
            Tables[TcpAcronyms.Airport].ElementRemoved +=
                (sender, args) =>
                {
                    var ids = RemoveFromAllItems(args);
                    Tables[TcpAcronyms.Flight].RemoveWhere(kvp =>
                    {
                        var flight = (Flight)kvp.Value;
                        return ids.Contains(flight.OriginId) || ids.Contains(flight.TargetId);
                    });
                };

            // Crew:
            Tables[TcpAcronyms.Crew].ElementRemoved +=
                (sender, args) =>
                {
                    var ids = RemoveFromAllItems(args);
                    foreach (var flight in Tables[TcpAcronyms.Flight].Items.Values.Cast<Flight>())
                    {
                        lock (flight.Lock)
                            flight.CrewIds = flight.CrewIds.Except(ids).ToArray();
                    }
                };

            // Cargo & Passenger:
            EventHandler<ElementRemovedEventArgs<IAviationItem>>? removeFromLoadIds =
                (sender, args) =>
                {
                    var ids = RemoveFromAllItems(args);
                    foreach (var flight in Tables[TcpAcronyms.Flight].Items.Values.Cast<Flight>())
                    {
                        lock (flight.Lock)
                            flight.LoadIds = flight.LoadIds.Except(ids).ToArray();
                    }
                };

            Tables[TcpAcronyms.Cargo].ElementRemoved += removeFromLoadIds;
            Tables[TcpAcronyms.Passenger].ElementRemoved += removeFromLoadIds;

            // PassengerPlane & CargoPlane:
            EventHandler<ElementRemovedEventArgs<IAviationItem>>? removeBasedOnPlaneId =
                (sender, args) =>
                {
                    var ids = RemoveFromAllItems(args);
                    Tables[TcpAcronyms.Flight].RemoveWhere(kvp =>
                    {
                        var flight = (Flight)kvp.Value;
                        return ids.Contains(flight.PlaneId);
                    });
                };
            Tables[TcpAcronyms.PassengerPlane].ElementRemoved += removeBasedOnPlaneId;
            Tables[TcpAcronyms.CargoPlane].ElementRemoved += removeBasedOnPlaneId;
        }
        private static HashSet<UInt64> RemoveFromAllItems(ElementRemovedEventArgs<IAviationItem> args)
        {
            var ids = args.RemovedElements.Select(x => { lock (x.Lock) return x.Id; }).ToHashSet();
            foreach (var id in ids)
                AllItems.Remove(id, out _);
            return ids;
        }

        private static List<IAviationItem> _cache { get; set; } = [];
        private static readonly object _cacheLock = new();
    }
}