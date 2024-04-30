using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.Tools;

namespace ProjOb_24L_01180781.Database.SQL.Visitors
{
    public static class QuerySetter
    {
        public delegate bool Setter<T>(T item, string value)
            where T : IAviationItem;

        public static readonly Dictionary<string, Setter<Crew>> CrewSetters = new(new KeyComparer())
        {
            { "ID",       (crew, value) => IdSetter(crew, value) },
            { "Name",     (crew, value) => StringSetter(ref crew.Name, value) },
            { "Age",      (crew, value) => UInt64Setter(ref crew.Age, value) },
            { "Phone",    (crew, value) => StringSetter(ref crew.PhoneNumber, value, Person.IsValidPhone) },
            { "Email",    (crew, value) => StringSetter(ref crew.EmailAddress, value, Person.IsValidEmail) },
            { "Practise", (crew, value) => UInt16Setter(ref crew.Practice, value) },
            { "Role",     (crew, value) => StringSetter(ref crew.Role, value) }
        };
        public static readonly Dictionary<string, Setter<Passenger>> PassengerSetters = new(new KeyComparer())
        {
            { "ID",    (passenger, value) => IdSetter(passenger, value) },
            { "Name",  (passenger, value) => StringSetter(ref passenger.Name, value) },
            { "Age",   (passenger, value) => UInt64Setter(ref passenger.Age, value) },
            { "Phone", (passenger, value) => StringSetter(ref passenger.PhoneNumber, value, Person.IsValidPhone) },
            { "Email", (passenger, value) => StringSetter(ref passenger.EmailAddress, value, Person.IsValidEmail) },
            { "Class", (passenger, value) => StringSetter(ref passenger.Class, value) },
            { "Miles", (passenger, value) => UInt64Setter(ref passenger.Miles, value) },
        };
        public static readonly Dictionary<string, Setter<PassengerPlane>> PassengerPlaneSetters = new(new KeyComparer())
        {
            { "ID",                (passengerPlane, value) => IdSetter(passengerPlane, value) },
            { "Serial",            (passengerPlane, value) => StringSetter(ref passengerPlane.Serial, value) },
            { "CountryCode",       (passengerPlane, value) => StringSetter(ref passengerPlane.Country, value) },
            { "Model",             (passengerPlane, value) => StringSetter(ref passengerPlane.Model, value) },
            { "FirstClassSize",    (passengerPlane, value) => UInt16Setter(ref passengerPlane.ClassSize.First, value) },
            { "BusinessClassSize", (passengerPlane, value) => UInt16Setter(ref passengerPlane.ClassSize.Business, value) },
            { "EconomyClassSize",  (passengerPlane, value) => UInt16Setter(ref passengerPlane.ClassSize.Economy, value) }
        };
        public static readonly Dictionary<string, Setter<CargoPlane>> CargoPlaneSetters = new(new KeyComparer())
        {
            { "ID",          (cargoPlane, value) => IdSetter(cargoPlane, value) },
            { "Serial",      (cargoPlane, value) => StringSetter(ref cargoPlane.Serial, value) },
            { "CountryCode", (cargoPlane, value) => StringSetter(ref cargoPlane.Country, value) },
            { "Model",       (cargoPlane, value) => StringSetter(ref cargoPlane.Model, value) },
            { "MaxLoad",     (cargoPlane, value) => SingleSetter(ref cargoPlane.MaxLoad, value, s => s > 0) }
        };
        public static readonly Dictionary<string, Setter<Cargo>> CargoSetters = new(new KeyComparer())
        {
            { "ID",           (cargo, value) => IdSetter(cargo, value) },
            { "Weight",       (cargo, value) => SingleSetter(ref cargo.Weight, value, s => s > 0) },
            { "Code",         (cargo, value) => StringSetter(ref cargo.Code, value) },
            { "Description",  (cargo, value) => StringSetter(ref cargo.Description, value) }
        };
        public static readonly Dictionary<string, Setter<Airport>> AirportSetters = new(new KeyComparer())
        {
            { "ID",                 (airport, value) => IdSetter(airport, value) },
            { "Name",               (airport, value) => StringSetter(ref airport.Name, value) },
            { "Code",               (airport, value) => StringSetter(ref airport.Code, value) },
            { "WorldPosition.Long", (airport, value) => DoubleSetter(ref airport.Position.Longitude, value, Position.IsValidLongitude) },
            { "WorldPosition.Lat",  (airport, value) => DoubleSetter(ref airport.Position.Latitude, value, Position.IsValidLatitude) },
            { "AMSL",               (airport, value) => DoubleSetter(ref airport.Position.Amsl, value) },
            { "CountryCode",        (airport, value) => StringSetter(ref airport.Country, value) },
        };
        public static readonly Dictionary<string, Setter<Flight>> FlightSetters = new(new KeyComparer())
        {
            { "ID",                 (flight, value) => IdSetter(flight, value) },
            { "Origin",             (flight, value) => UInt64Setter(ref flight.OriginId, value) },
            { "Target",             (flight, value) => UInt64Setter(ref flight.TargetId, value) },
            { "TakeOffTime",        (flight, value) => DateTimeSetter(ref flight.TakeOffDateTime, value) },
            { "LandingTime",        (flight, value) => DateTimeSetter(ref flight.LandingDateTime, value) },
            { "WorldPosition.Long", (flight, value) =>
                {
                    double parsed = 0;
                    bool result;
                    if(result = DoubleSetter(ref parsed, value, Position.IsValidLongitude))
                        flight.UpdatePosition(longitude : parsed);
                    return result;
                }
            },
            { "WorldPosition.Lat", (flight, value) =>
                {
                    double parsed = 0;
                    bool result;
                    if(result = DoubleSetter(ref parsed, value, Position.IsValidLatitude))
                        flight.UpdatePosition(latitude : parsed);
                    return result;
                }
            },
            { "AMSL", (flight, value) =>
                {
                    double parsed = 0;
                    bool result;
                    if(result = DoubleSetter(ref parsed, value))
                        flight.UpdatePosition(amsl : parsed);
                    return result;
                }
            },
            { "Plane",              (flight, value) => UInt64Setter(ref flight.PlaneId, value) }
        };

        public static bool StringSetter(ref string? field, string value, Func<string, bool>? predicate = null)
        {
            predicate ??= s => true;
            if (!predicate(value)) return false;
            field = value;
            return true;
        }
        public static bool SingleSetter(ref float field, string value, Func<float, bool>? predicate = null)
        {
            predicate ??= s => true;
            if (float.TryParse(value, out var parsed) && predicate(parsed))
            {
                field = parsed;
                return true;
            }
            return false;
        }
        public static bool DoubleSetter(ref double field, string value, Func<double, bool>? predicate = null)
        {
            predicate ??= s => true;
            if (double.TryParse(value, out var parsed) && predicate(parsed))
            {
                field = parsed;
                return true;
            }
            return false;
        }
        public static bool UInt64Setter(ref ulong field, string value)
        {
            if (ulong.TryParse(value, out var parsed))
            {
                field = parsed;
                return true;
            }
            return false;
        }
        public static bool UInt16Setter(ref ushort field, string value, Func<ushort, bool>? predicate = null)
        {
            predicate ??= s => true;
            if (ushort.TryParse(value, out var parsed) && predicate(parsed))
            {
                field = parsed;
                return true;
            }
            return false;
        }
        public static bool DateTimeSetter(ref DateTime field, string value)
        {
            if (DateTime.TryParse(value, out var parsed))
            {
                field = parsed;
                return true;
            }
            return false;
        }
        public static bool IdSetter<T>(T item, string value, Func<ulong, bool>? predicate = null)
            where T : IAviationItem
        {
            if (ulong.TryParse(value, out var parsed))
            {
                item.Id = parsed;
                return true;
            }
            return false;
        }
    }
}