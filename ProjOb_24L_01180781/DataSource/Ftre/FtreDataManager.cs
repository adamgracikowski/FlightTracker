#define DEBUGLOG

using DynamicData;
using NetworkSourceSimulator;
using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.Database;
using ProjOb_24L_01180781.DataSource.Ftr;
using ProjOb_24L_01180781.DataSource.Tcp;
using Nss = NetworkSourceSimulator;

namespace ProjOb_24L_01180781.DataSource.Ftre
{
    public class FtreDataManager : DataManager
    {
        public Task RunNetworkSource(Nss.NetworkSourceSimulator networkSource)
        {
            return Task.Factory.StartNew(networkSource.Run);
        }
        public void SubscribeToNetworkSource(Nss.NetworkSourceSimulator networkSource)
        {
            networkSource.OnIDUpdate += UpdateId;
            networkSource.OnPositionUpdate += UpdatePosition;
            networkSource.OnContactInfoUpdate += UpdateContactInfo;
        }

        private static void UpdateId(object? sender, IDUpdateArgs args)
        {
            string status;
            var item = AviationDatabase.Find(args.ObjectID);
            var shouldBeNull = AviationDatabase.Find(args.NewObjectID);

            if (item is null || shouldBeNull is not null)
                status = UpdateStatus.Failure;
            else if (_idUpdateDictionary.TryGetValue(item.FtrAcronym, out var callback) && callback is not null)
                status = callback(args, item) ? UpdateStatus.Success : UpdateStatus.Failure;
            else
                status = UpdateStatus.Failure;

            var log = $"{status} | {args.ObjectID}; {args.NewObjectID}";

            DebugLog(log);
            _logManager.Write(log);
        }
        private static void UpdatePosition(object? sender, PositionUpdateArgs args)
        {
            string log;
            var item = AviationDatabase.Find(args.ObjectID);
            if (item is null || !(item.FtrAcronym == FtrAcronyms.Airport || item.FtrAcronym == FtrAcronyms.Flight))
            {
                log = $"{UpdateStatus.Failure} | {args.ObjectID}; " +
                    $"{args.Longitude}; {args.Latitude}; {args.AMSL}";
            }
            else
            {
                var positionable = (IPositionable)item;
                lock (item.Lock)
                {
                    var oldPosition = positionable.GetPosition();
                    if (Position.IsValidLatitude(args.Latitude) && Position.IsValidLongitude(args.Longitude))
                    {
                        positionable.UpdatePosition(args.Longitude, args.Latitude, args.AMSL);

                        log = $"{UpdateStatus.Success} | {args.ObjectID}; " +
                            $"{oldPosition.Longitude} --> {args.Longitude}; " +
                            $"{oldPosition.Latitude} --> {args.Latitude}; " +
                            $"{oldPosition.Amsl} --> {args.AMSL}";
                    }
                    else
                    {
                        log = $"{UpdateStatus.Failure} | {args.ObjectID}; " +
                            $"{args.Longitude}; {args.Latitude}; {args.AMSL}";
                    }
                }
            }

            DebugLog(log);
            _logManager.Write(log);
        }
        private static void UpdateContactInfo(object? sender, ContactInfoUpdateArgs args)
        {
            string log;
            var item = AviationDatabase.Find(args.ObjectID);
            if (item is null || !(item.FtrAcronym == FtrAcronyms.Passenger || item.FtrAcronym == FtrAcronyms.Crew))
                log = $"{UpdateStatus.Failure} | {args.ObjectID}; " +
                    $"{args.PhoneNumber}; {args.EmailAddress}";
            else
            {
                var contactable = (IContactable)item;
                lock (item.Lock)
                {
                    var oldPhone = contactable.Phone;
                    var oldEmail = contactable.Email;
                    if (Person.IsValidEmail(args.EmailAddress) && Person.IsValidPhone(args.PhoneNumber))
                    {
                        contactable.Phone = args.PhoneNumber;
                        contactable.Email = args.EmailAddress;
                        log = $"{UpdateStatus.Success} | {args.ObjectID}; " +
                            $"{oldPhone} --> {args.PhoneNumber}; " +
                            $"{oldEmail} --> {args.EmailAddress}";
                    }
                    else
                    {
                        log = $"{UpdateStatus.Failure} | {args.ObjectID}; " +
                            $"{args.PhoneNumber}; {args.EmailAddress}";
                    }
                }
            }

            DebugLog(log);
            _logManager.Write(log);
        }
        private static bool UpdateAirportId(IDUpdateArgs args, IAviationItem item)
        {
            UInt64 oldId = args.ObjectID, newId = args.NewObjectID;
            if (!UpdateId(oldId, newId, item, FtrAcronyms.Airport))
                return false;

            foreach (var flight in AviationDatabase.Tables[TcpAcronyms.Flight].Items.Values.Cast<Flight>())
            {
                lock (flight.Lock)
                {
                    if (flight.OriginId == oldId)
                        flight.OriginId = newId;
                    if (flight.TargetId == oldId)
                        flight.TargetId = newId;
                }
            }

            return RemoveAndAdd(oldId, AviationDatabase.Tables[TcpAcronyms.Airport]);
        }
        private static bool UpdateLoadId(IDUpdateArgs args, IAviationItem item)
        {
            UInt64 oldId = args.ObjectID, newId = args.NewObjectID;
            if (!UpdateId(oldId, newId, item, FtrAcronyms.Cargo, FtrAcronyms.Passenger))
                return false;

            foreach (var flight in AviationDatabase.Tables[TcpAcronyms.Flight].Items.Values.Cast<Flight>())
            {
                lock (flight.Lock)
                    flight.LoadIds = Array.ConvertAll(flight.LoadIds, id => id == oldId ? newId : id);
            }

            if (item.FtrAcronym == FtrAcronyms.Cargo)
                return RemoveAndAdd(oldId, AviationDatabase.Tables[TcpAcronyms.Cargo]);
            return RemoveAndAdd(oldId, AviationDatabase.Tables[TcpAcronyms.Passenger]);
        }
        private static bool UpdatePlaneId(IDUpdateArgs args, IAviationItem item)
        {
            UInt64 oldId = args.ObjectID, newId = args.NewObjectID;
            if (!UpdateId(oldId, newId, item, FtrAcronyms.CargoPlane, FtrAcronyms.PassengerPlane))
                return false;

            foreach (var flight in AviationDatabase.Tables[TcpAcronyms.Flight].Items.Values.Cast<Flight>())
            {
                lock (flight.Lock)
                {
                    if (flight.PlaneId == oldId)
                        flight.PlaneId = newId;
                }
            }

            if (item.FtrAcronym == FtrAcronyms.CargoPlane)
                return RemoveAndAdd(oldId, AviationDatabase.Tables[TcpAcronyms.CargoPlane]);
            return RemoveAndAdd(oldId, AviationDatabase.Tables[TcpAcronyms.PassengerPlane]);
        }
        private static bool UpdateCrewId(IDUpdateArgs args, IAviationItem item)
        {
            UInt64 oldId = args.ObjectID, newId = args.NewObjectID;
            if (!UpdateId(oldId, newId, item, FtrAcronyms.Crew))
                return false;

            foreach (var flight in AviationDatabase.Tables[TcpAcronyms.Flight].Items.Values.Cast<Flight>())
            {
                lock (flight.Lock)
                    flight.CrewIds = Array.ConvertAll(flight.CrewIds, id => id == oldId ? newId : id);
            }
            return RemoveAndAdd(oldId, AviationDatabase.Tables[TcpAcronyms.Crew]);
        }
        private static bool UpdateFlightId(IDUpdateArgs args, IAviationItem item)
        {
            if (!UpdateId(args.ObjectID, args.NewObjectID, item, FtrAcronyms.Flight))
                return false;
            return RemoveAndAdd(args.ObjectID, AviationDatabase.Tables[TcpAcronyms.Flight]);
        }
        private static bool UpdateId(UInt64 oldId, UInt64 newId, IAviationItem item, params string[] ftrAcronym)
        {
            lock (item.Lock)
            {
                if (item.Id != oldId || !ftrAcronym.Contains(item.FtrAcronym))
                    return false;
                item.UpdateId(newId);
            }
            return true;
        }
        private static bool RemoveAndAdd<T>(UInt64 id, DatabaseTable<T> table)
            where T : class?, IAviationItem
        {
            if (!table.Items.Remove(id, out var item) || item is null)
                return false;
            if (!AviationDatabase.AllItems.Remove(id, out _))
                return false;

            lock (item.Lock)
            {
                if (!table.Items.TryAdd(item.Id, item))
                    return false;
                if (!AviationDatabase.AllItems.TryAdd(item.Id, item))
                {
                    table.Items.Remove(item.Id, out _);
                    return false;
                }
            }

            return true;
        }

        private static readonly Dictionary<string, Func<IDUpdateArgs, IAviationItem, bool>> _idUpdateDictionary = new()
        {
            { FtrAcronyms.Airport,        UpdateAirportId },
            { FtrAcronyms.Crew,           UpdateCrewId },
            { FtrAcronyms.Cargo,          UpdateLoadId },
            { FtrAcronyms.CargoPlane,     UpdatePlaneId },
            { FtrAcronyms.Flight,         UpdateFlightId },
            { FtrAcronyms.Passenger,      UpdateLoadId },
            { FtrAcronyms.PassengerPlane, UpdatePlaneId }
        };
        private static LogManager _logManager { get; set; } = LogManager.GetInstance();

        private static void DebugLog(string log)
        {
#if DEBUGLOG
            Console.WriteLine(log);
#endif // Debug
        }
    }
}