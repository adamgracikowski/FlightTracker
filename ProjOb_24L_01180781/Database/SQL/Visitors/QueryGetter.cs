using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.Tools;


namespace ProjOb_24L_01180781.Database.SQL.Visitors
{
    public static class QueryGetter
    {
        public delegate string? Getter<T>(T item)
            where T : IAviationItem;

        public static readonly Dictionary<string, Getter<Airport>> AirportGetters = new(new KeyComparer())
        {
            { "ID", airport => airport.Id.ToString() },
            { "Name", airport => airport.Name },
            { "Code", airport => airport.Code },
            { "WorldPosition.Long", airport => airport.Position.Longitude.ToString() },
            { "WorldPosition.Lat", airport => airport.Position.Latitude.ToString() },
            { "AMSL", airport => airport.Position.Amsl.ToString() },
            { "CountryCode", airport => airport.Country },
        };
        public static readonly Dictionary<string, Getter<Flight>> FlightGetters = new(new KeyComparer())
        {
            { "ID",                 flight => flight.Id.ToString() },
            { "Origin",             flight => flight.OriginId.ToString() },
            { "Target",             flight => flight.TargetId.ToString() },
            { "TakeOffTime",        flight => flight.TakeOffDateTime.ToString() },
            { "LandingTime",        flight => flight.LandingDateTime.ToString() },
            { "WorldPosition.Long", flight => flight.Position.Longitude.ToString() },
            { "WorldPosition.Lat",  flight => flight.Position.Latitude.ToString() },
            { "AMSL",               flight => flight.Position.Amsl.ToString() },
            { "Plane",              flight => flight.PlaneId.ToString() }
        };
        public static readonly Dictionary<string, Getter<Crew>> CrewGetters = new(new KeyComparer())
        {
            { "ID",       crew => crew.Id.ToString() },
            { "Name",     crew => crew.Name },
            { "Age",      crew => crew.Age.ToString() },
            { "Phone",    crew => crew.Phone },
            { "Email",    crew => crew.EmailAddress },
            { "Practise", crew => crew.PhoneNumber },
            { "Role",     crew => crew.Role }
        };
        public static readonly Dictionary<string, Getter<Passenger>> PassengerGetters = new(new KeyComparer())
        {
            { "ID",    passenger => passenger.Id.ToString() },
            { "Name",  passenger => passenger.Name },
            { "Age",   passenger => passenger.Age.ToString() },
            { "Phone", passenger => passenger.PhoneNumber },
            { "Email", passenger => passenger.EmailAddress },
            { "Class", passenger => passenger.Class },
            { "Miles", passenger => passenger.Miles.ToString() }
        };
        public static readonly Dictionary<string, Getter<PassengerPlane>> PassengerPlaneGetters = new(new KeyComparer())
        {
            { "ID",                passengerPlane => passengerPlane.Id.ToString() },
            { "Serial",            passengerPlane => passengerPlane.Serial },
            { "CountryCode",       passengerPlane => passengerPlane.Country },
            { "Model",             passengerPlane => passengerPlane.Model },
            { "FirstClassSize",    passengerPlane => passengerPlane.ClassSize.First.ToString() },
            { "BusinessClassSize", passengerPlane => passengerPlane.ClassSize.Business.ToString() },
            { "EconomyClassSize",  passengerPlane => passengerPlane.ClassSize.Economy.ToString() },
        };
        public static readonly Dictionary<string, Getter<CargoPlane>> CargoPlaneGetters = new(new KeyComparer())
        {
            { "ID",          cargoPlane => cargoPlane.Id.ToString() },
            { "Serial",      cargoPlane => cargoPlane.Serial },
            { "CountryCode", cargoPlane => cargoPlane.Country },
            { "Model",       cargoPlane => cargoPlane.Model },
            { "MaxLoad",     cargoPlane => cargoPlane.MaxLoad.ToString() }
        };
        public static readonly Dictionary<string, Getter<Cargo>> CargoGetters = new(new KeyComparer())
        {
            { "ID",           cargo => cargo.Id.ToString() },
            { "Weight",       cargo => cargo.Weight.ToString() },
            { "Code",         cargo => cargo.Code },
            { "Description",  cargo => cargo.Description }
        };
    }
}