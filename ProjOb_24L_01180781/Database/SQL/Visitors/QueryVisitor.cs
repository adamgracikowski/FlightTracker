using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.AviationItems.Interfaces;
using static ProjOb_24L_01180781.Database.SQL.Visitors.QuerySetter;

namespace ProjOb_24L_01180781.Database.SQL.Visitors
{
    public abstract class QueryVisitor
    {
        public abstract void RunQuery(Airport airport);
        public abstract void RunQuery(Cargo cargo);
        public abstract void RunQuery(CargoPlane cargoPlane);
        public abstract void RunQuery(Crew crew);
        public abstract void RunQuery(Flight flight);
        public abstract void RunQuery(Passenger passenger);
        public abstract void RunQuery(PassengerPlane passengerPlane);

        protected static void AssignmentLoop<T>(T item, Dictionary<string, Setter<T>> setters,
            List<Assignment> assignments)
            where T : IAviationItem
        {
            foreach (var assignment in assignments)
            {
                if (setters.TryGetValue(assignment.Field, out var setter))
                {
                    if (!setter(item, assignment.Value))
                        throw new FormatException($"Invalid assignment: {assignment}");
                }
            }
        }
        protected static void CheckRequired(HashSet<string> fields, params string[] required)
        {
            foreach (var field in required)
            {
                if (!fields.Contains(field))
                    throw new FormatException($"Column requirements not met ({field}).");
            }
        }
        protected static void CheckForbidden(HashSet<string> fields, params string[] forbidden)
        {
            foreach (var field in forbidden)
            {
                if (fields.Contains(field))
                    throw new FormatException($"Forbidden column modification ({field}).");
            }
        }
    }
}
