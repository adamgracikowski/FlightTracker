using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.DataSource.Tcp;
using ProjOb_24L_01180781.Tools;

namespace ProjOb_24L_01180781.Database.SQL.WhereClause
{
    public class WhereAirport
        : WhereBase<Airport>, IWhereEvaluator
    {
        public WhereAirport(List<WhereCondition> whereConditions, List<string> conjunctions)
            : base(whereConditions, conjunctions)
        {
            FiltersDictionary = new(new KeyComparer())
            {
                { "ID",                 (airport, value, comparison) => ParseCompare(airport.Id, value, comparison) },
                { "Name",               (airport, value, comparison) => Compare(airport.Name, value, comparison) },
                { "Code",               (airport, value, comparison) => Compare(airport.Code, value, comparison) },
                { "WorldPosition.Long", (airport, value, comparison) => ParseCompare(airport.Position.Longitude, value, comparison) },
                { "WorldPosition.Lat",  (airport, value, comparison) => ParseCompare(airport.Position.Latitude, value, comparison) },
                { "AMSL",               (airport, value, comparison) => ParseCompare(airport.Position.Amsl, value, comparison) },
                { "CountryCode",        (airport, value, comparison) => Compare(airport.Country, value, comparison) }
            };
            FetchFilters();
        }
        public bool Evaluate(IAviationItem item)
        {
            if (item.TcpAcronym != TcpAcronyms.Airport)
                throw new FormatException("Inconsistent data.");
            var airport = (Airport)item;

            return EvaluateFilters(airport);
        }
    }
    public class WherePassengerPlane
        : WhereBase<PassengerPlane>, IWhereEvaluator
    {
        public WherePassengerPlane(List<WhereCondition> whereConditions, List<string> conjunctions)
            : base(whereConditions, conjunctions)
        {
            FiltersDictionary = new(new KeyComparer())
            {
                { "ID",                (passengerPlane, value, comparison) => ParseCompare(passengerPlane.Id, value, comparison) },
                { "Serial",            (passengerPlane, value, comparison) => Compare(passengerPlane.Serial, value, comparison) },
                { "Country",           (passengerPlane, value, comparison) => Compare(passengerPlane.Country, value, comparison) },
                { "Model",             (passengerPlane, value, comparison) => Compare(passengerPlane.Model, value, comparison) },
                { "FirstClassSize",    (passengerPlane, value, comparison) => ParseCompare(passengerPlane.ClassSize.First, value, comparison) },
                { "BusinessClassSize", (passengerPlane, value, comparison) => ParseCompare(passengerPlane.ClassSize.Business, value, comparison) },
                { "EconomyClassSize",  (passengerPlane, value, comparison) => ParseCompare(passengerPlane.ClassSize.Economy, value, comparison) }
            };
            FetchFilters();
        }
        public bool Evaluate(IAviationItem item)
        {
            if (item.TcpAcronym != TcpAcronyms.PassengerPlane)
                throw new FormatException("Inconsistent data.");
            var passengerPlane = (PassengerPlane)item;

            return EvaluateFilters(passengerPlane);
        }
    }
    public class WhereCargoPlane
        : WhereBase<CargoPlane>, IWhereEvaluator
    {
        public WhereCargoPlane(List<WhereCondition> whereConditions, List<string> conjunctions)
            : base(whereConditions, conjunctions)
        {
            FiltersDictionary = new(new KeyComparer())
            {
                { "ID",      (cargoPlane, value, comparison) => ParseCompare(cargoPlane.Id, value, comparison) },
                { "Serial",  (cargoPlane, value, comparison) => Compare(cargoPlane.Serial, value, comparison) },
                { "Country", (cargoPlane, value, comparison) => Compare(cargoPlane.Country, value, comparison) },
                { "Model",   (cargoPlane, value, comparison) => Compare(cargoPlane.Model, value, comparison) },
                { "MaxLoad", (cargoPlane, value, comparison) => ParseCompare(cargoPlane.MaxLoad, value, comparison) }
            };
            FetchFilters();
        }

        public bool Evaluate(IAviationItem item)
        {
            if (item.TcpAcronym != TcpAcronyms.CargoPlane)
                throw new FormatException("Inconsistent data.");
            var cargoPlane = (CargoPlane)item;

            return EvaluateFilters(cargoPlane);
        }
    }
    public class WhereCargo
    : WhereBase<Cargo>, IWhereEvaluator
    {
        public WhereCargo(List<WhereCondition> whereConditions, List<string> conjunctions)
            : base(whereConditions, conjunctions)
        {
            FiltersDictionary = new(new KeyComparer())
            {
                { "ID",          (cargo, value, comparison) => ParseCompare(cargo.Id, value, comparison) },
                { "Weight",      (cargo, value, comparison) => ParseCompare(cargo.Weight, value, comparison) },
                { "Code",        (cargo, value, comparison) => Compare(cargo.Code, value, comparison) },
                { "Description", (cargo, value, comparison) => Compare(cargo.Description, value, comparison) }
            };
            FetchFilters();
        }

        public bool Evaluate(IAviationItem item)
        {
            if (item.TcpAcronym != TcpAcronyms.Cargo)
                throw new FormatException("Inconsistent data.");
            var cargo = (Cargo)item;

            return EvaluateFilters(cargo);
        }
    }
    public class WherePassenger
        : WhereBase<Passenger>, IWhereEvaluator
    {
        public WherePassenger(List<WhereCondition> whereConditions, List<string> conjunctions)
            : base(whereConditions, conjunctions)
        {
            FiltersDictionary = new(new KeyComparer())
            {
                { "ID",    (passenger, value, comparison) => ParseCompare(passenger.Id, value, comparison) },
                { "Name",  (passenger, value, comparison) => Compare(passenger.Name, value, comparison) },
                { "Age",   (passenger, value, comparison) => ParseCompare(passenger.Age, value, comparison) },
                { "Phone", (passenger, value, comparison) => Compare(passenger.Phone, value, comparison) },
                { "Email", (passenger, value, comparison) => Compare(passenger.Email, value, comparison) },
                { "Class", (passenger, value, comparison) => Compare(passenger.Class, value, comparison) },
                { "Miles", (passenger, value, comparison) => ParseCompare(passenger.Miles, value, comparison) }
            };
            FetchFilters();
        }

        public bool Evaluate(IAviationItem item)
        {
            if (item.TcpAcronym != TcpAcronyms.Passenger)
                throw new FormatException("Inconsistent data.");
            var passenger = (Passenger)item;

            return EvaluateFilters(passenger);
        }
    }
    public class WhereCrew
    : WhereBase<Crew>, IWhereEvaluator
    {
        public WhereCrew(List<WhereCondition> whereConditions, List<string> conjunctions)
            : base(whereConditions, conjunctions)
        {
            FiltersDictionary = new(new KeyComparer())
            {
                { "ID",       (crew, value, comparison) => ParseCompare(crew.Id, value, comparison) },
                { "Name",     (crew, value, comparison) => Compare(crew.Name, value, comparison) },
                { "Age",      (crew, value, comparison) => ParseCompare(crew.Age, value, comparison) },
                { "Phone",    (crew, value, comparison) => Compare(crew.Phone, value, comparison) },
                { "Email",    (crew, value, comparison) => Compare(crew.Email, value, comparison) },
                { "Practise", (crew, value, comparison) => ParseCompare(crew.Practice, value, comparison) },
                { "Role",     (crew, value, comparison) => Compare(crew.Role, value, comparison) }
            };
            FetchFilters();
        }

        public bool Evaluate(IAviationItem item)
        {
            if (item.TcpAcronym != TcpAcronyms.Crew)
                throw new FormatException("Inconsistent data.");
            var crew = (Crew)item;

            return EvaluateFilters(crew);
        }
    }
    public class WhereFlight
        : WhereBase<Flight>, IWhereEvaluator
    {
        public WhereFlight(List<WhereCondition> whereConditions, List<string> conjunctions)
            : base(whereConditions, conjunctions)
        {
            FiltersDictionary = new(new KeyComparer())
            {
                { "ID",                 (flight, value, comparison) => ParseCompare(flight.Id, value, comparison) },
                { "Origin",             (flight, value, comparison) => ParseCompare(flight.OriginId, value, comparison) },
                { "Target",             (flight, value, comparison) => ParseCompare(flight.TargetId, value, comparison) },
                { "TakeOffTime",        (flight, value, comparison) => ParseCompare(flight.TakeOffDateTime, value, comparison) },
                { "LandingTime",        (flight, value, comparison) => ParseCompare(flight.LandingDateTime, value, comparison) },
                { "WorldPosition.Long", (flight, value, comparison) => ParseCompare(flight.Position.Longitude, value, comparison) },
                { "WorldPosition.Lat",  (flight, value, comparison) => ParseCompare(flight.Position.Latitude, value, comparison) },
                { "AMSL",               (flight, value, comparison) => ParseCompare(flight.Position.Amsl, value, comparison) },
                { "Plane",              (flight, value, comparison) => ParseCompare(flight.PlaneId, value, comparison) }
            };
            FetchFilters();
        }

        public bool Evaluate(IAviationItem item)
        {
            if (item.TcpAcronym != TcpAcronyms.Flight)
                throw new FormatException("Inconsistent data.");
            var flight = (Flight)item;

            return EvaluateFilters(flight);
        }
    }
}