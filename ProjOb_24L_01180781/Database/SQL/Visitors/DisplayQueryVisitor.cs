using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.Database.SQL.Queries;
using ProjOb_24L_01180781.Database.SQL.WhereClause;
using ProjOb_24L_01180781.Tools;
using static ProjOb_24L_01180781.Database.SQL.Visitors.QueryGetter;


namespace ProjOb_24L_01180781.Database.SQL.Visitors
{
    public class DisplayQueryVisitor : QueryVisitor
    {
        public DisplayQuery DisplayQuery { get; set; }
        public List<List<string>> Data { get => [.. DataDictionary.Values]; }
        public IWhereEvaluator? WhereEvaluator { get; set; }
        public DisplayQueryVisitor(DisplayQuery displayQuery)
        {
            DisplayQuery = displayQuery;
            if (!QueryColumns.Dictionary.TryGetValue(DisplayQuery.TableName, out var hashSet))
                throw new FormatException($"Invalid table name ({DisplayQuery.TableName}).");

            if (displayQuery.Columns.Count == 1 && displayQuery.Columns[0] == StarWildCard)
            {
                foreach (var column in hashSet)
                {
                    DataDictionary.Add(column, [column]);
                }
                displayQuery.Columns = [.. hashSet];
            }
            else
            {
                foreach (var column in displayQuery.Columns)
                {
                    if (!hashSet.Contains(column))
                        throw new FormatException($"Invalid column name ({column}).");
                    DataDictionary.Add(column, [column]);
                }
            }
            WhereEvaluator = WhereGenerator.Evaluators[DisplayQuery.TableName](DisplayQuery.WhereConditions, DisplayQuery.Conjunctions);
        }

        public override void RunQuery(Airport airport)
        {
            DataLoop(airport, AirportGetters);
        }
        public override void RunQuery(Cargo cargo)
        {
            DataLoop(cargo, CargoGetters);
        }
        public override void RunQuery(CargoPlane cargoPlane)
        {
            DataLoop(cargoPlane, CargoPlaneGetters);
        }
        public override void RunQuery(Crew crew)
        {
            DataLoop(crew, CrewGetters);
        }
        public override void RunQuery(Flight flight)
        {
            DataLoop(flight, FlightGetters);
        }
        public override void RunQuery(Passenger passenger)
        {
            DataLoop(passenger, PassengerGetters);
        }
        public override void RunQuery(PassengerPlane passengerPlane)
        {
            DataLoop(passengerPlane, PassengerPlaneGetters);
        }

        private void DataLoop<T>(T item, Dictionary<string, Getter<T>> actions)
            where T : IAviationItem
        {
            foreach (var column in DisplayQuery.Columns)
            {
                var field = actions[column](item);
                DataDictionary[column].Add(field ?? string.Empty);
            }
        }
        private Dictionary<string, List<string>> DataDictionary { get; set; } = new(new KeyComparer());
        private static readonly string StarWildCard = "*";
    }
}