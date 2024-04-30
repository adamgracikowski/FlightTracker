using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.Tools;

namespace ProjOb_24L_01180781.Database.SQL.WhereClause
{
    public static class WhereGenerator
    {
        public static readonly Dictionary<string, Func<List<WhereCondition>, List<string>, IWhereEvaluator>> Evaluators
            = new(new KeyComparer())
        {
            { AviationName.Airport,        (conditions, conjunctions) => new WhereAirport(conditions, conjunctions) },
            { AviationName.Crew,           (conditions, conjunctions) => new WhereCrew(conditions, conjunctions) },
            { AviationName.Cargo,          (conditions, conjunctions) => new WhereCargo(conditions, conjunctions) },
            { AviationName.CargoPlane,     (conditions, conjunctions) => new WhereCargoPlane(conditions, conjunctions) },
            { AviationName.Passenger,      (conditions, conjunctions) => new WherePassenger(conditions, conjunctions) },
            { AviationName.PassengerPlane, (conditions, conjunctions) => new WherePassengerPlane(conditions, conjunctions) },
            { AviationName.Flight,         (conditions, conjunctions) => new WhereFlight(conditions, conjunctions) }
        };
    }
}