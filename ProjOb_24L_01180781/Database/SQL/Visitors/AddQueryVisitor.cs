using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.Database.SQL.Queries;
using ProjOb_24L_01180781.DataSource.Tcp;
using ProjOb_24L_01180781.Tools;
using static ProjOb_24L_01180781.Database.SQL.Visitors.QuerySetter;

namespace ProjOb_24L_01180781.Database.SQL.Visitors
{
    public class AddQueryVisitor : QueryVisitor
    {
        public AddQuery AddQuery { get; set; }
        public HashSet<string> Fields { get; set; }
        public AddQueryVisitor(AddQuery addQuery)
        {
            AddQuery = addQuery;
            Fields = AddQuery.Assignments.Select(a => a.Field).ToHashSet(new KeyComparer());
        }

        public override void RunQuery(Airport airport)
        {
            CheckRequired(Fields, "ID");
            AssignmentLoop(airport, AirportSetters, AddQuery.Assignments);
        }
        public override void RunQuery(Cargo cargo)
        {
            CheckRequired(Fields, "ID");
            AssignmentLoop(cargo, CargoSetters, AddQuery.Assignments);
        }
        public override void RunQuery(CargoPlane cargoPlane)
        {
            CheckRequired(Fields, "ID");
            AssignmentLoop(cargoPlane, CargoPlaneSetters, AddQuery.Assignments);
        }
        public override void RunQuery(Crew crew)
        {
            CheckRequired(Fields, "ID");
            AssignmentLoop(crew, CrewSetters, AddQuery.Assignments);
        }
        public override void RunQuery(Flight flight)
        {
            CheckRequired(Fields, "ID", "Origin", "Target", "Plane", "TakeOffTime", "LandingTime");
            AssignmentLoop(flight, FlightSetters, AddQuery.Assignments);

            if (!AviationDatabase.Tables[TcpAcronyms.Airport].Items.ContainsKey(flight.OriginId) ||
                !AviationDatabase.Tables[TcpAcronyms.Airport].Items.ContainsKey(flight.TargetId) ||
                !(AviationDatabase.Tables[TcpAcronyms.PassengerPlane].Items.ContainsKey(flight.PlaneId) ||
                  AviationDatabase.Tables[TcpAcronyms.CargoPlane].Items.ContainsKey(flight.PlaneId)))
            {
                throw new FormatException("Invalid foreign key.");
            }
        }
        public override void RunQuery(Passenger passenger)
        {
            CheckRequired(Fields, "ID");
            AssignmentLoop(passenger, PassengerSetters, AddQuery.Assignments);
        }
        public override void RunQuery(PassengerPlane passengerPlane)
        {
            CheckRequired(Fields, "ID");
            AssignmentLoop(passengerPlane, PassengerPlaneSetters, AddQuery.Assignments);
        }
    }
}