using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.Database.SQL.Queries;
using ProjOb_24L_01180781.Database.SQL.WhereClause;
using ProjOb_24L_01180781.Tools;
using static ProjOb_24L_01180781.Database.SQL.Visitors.QuerySetter;

namespace ProjOb_24L_01180781.Database.SQL.Visitors
{
    public class UpdateQueryVisitor : QueryVisitor
    {
        public UpdateQueryVisitor(UpdateQuery updateQuery)
        {
            UpdateQuery = updateQuery;
            Fields = UpdateQuery.Assignments.Select(a => a.Field).ToHashSet(new KeyComparer());

            if (WhereGenerator.Evaluators.TryGetValue(UpdateQuery.TableName, out var generator))
                WhereEvaluator = generator(UpdateQuery.WhereConditions, UpdateQuery.Conjunctions);
            else throw new FormatException($"Invalid table name ({UpdateQuery.TableName}).");
        }

        public IWhereEvaluator? WhereEvaluator { get; set; }
        public UpdateQuery UpdateQuery { get; set; }
        public HashSet<string> Fields { get; set; }
        public long UpdateCounter { get; set; }

        public override void RunQuery(Airport airport)
        {
            CheckForbidden(Fields, "ID");
            AssignmentLoop(airport, AirportSetters, UpdateQuery.Assignments);
            UpdateCounter++;
        }
        public override void RunQuery(Cargo cargo)
        {
            CheckForbidden(Fields, "ID");
            AssignmentLoop(cargo, CargoSetters, UpdateQuery.Assignments);
            UpdateCounter++;
        }
        public override void RunQuery(CargoPlane cargoPlane)
        {
            CheckForbidden(Fields, "ID");
            AssignmentLoop(cargoPlane, CargoPlaneSetters, UpdateQuery.Assignments);
            UpdateCounter++;
        }
        public override void RunQuery(Crew crew)
        {
            CheckForbidden(Fields, "ID");
            AssignmentLoop(crew, CrewSetters, UpdateQuery.Assignments);
            UpdateCounter++;
        }
        public override void RunQuery(Flight flight)
        {
            CheckForbidden(Fields, "ID", "Origin", "Target", "Plane");
            AssignmentLoop(flight, FlightSetters, UpdateQuery.Assignments);
            UpdateCounter++;
        }
        public override void RunQuery(Passenger passenger)
        {
            CheckForbidden(Fields, "ID");
            AssignmentLoop(passenger, PassengerSetters, UpdateQuery.Assignments);
            UpdateCounter++;
        }
        public override void RunQuery(PassengerPlane passengerPlane)
        {
            CheckForbidden(Fields, "ID");
            AssignmentLoop(passengerPlane, PassengerPlaneSetters, UpdateQuery.Assignments);
            UpdateCounter++;
        }
    }
}