using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.Database.SQL.Visitors;
using ProjOb_24L_01180781.Database.SQL.WhereClause;
using ProjOb_24L_01180781.DataSource.Tcp;

namespace ProjOb_24L_01180781.Database.SQL
{
    public static class QueryInterpreter
    {
        public delegate IAviationItem Generator();

        public static IAviationItem InterpretAddQuery(string query)
        {
            var addQuery = QueryParser.ParseAddQuery(query)
                ?? throw new FormatException("Query parsing failure.");
            var table = InterpretTableName(addQuery.TableName);

            var visitor = new AddQueryVisitor(addQuery);
            var item = GeneratorDictionary[table.Acronym]();
            item.AcceptQueryVisitor(visitor);

            return item;
        }
        public static List<List<string>> InterpretDisplayQuery(string query)
        {
            var displayQuery = QueryParser.ParseDisplayQuery(query)
                ?? throw new FormatException("Query parsing failure.");
            var table = InterpretTableName(displayQuery.TableName);

            var visitor = new DisplayQueryVisitor(displayQuery);

            if (visitor.WhereEvaluator is null)
                throw new FormatException("Invalid where clause.");

            foreach (var item in table.Items.Values)
            {
                lock (item.Lock)
                {
                    if (visitor.WhereEvaluator.Evaluate(item))
                        item.AcceptQueryVisitor(visitor);
                }
            }
            return visitor.Data;
        }
        public static long InterpretUpdateQuery(string query)
        {
            var updateQuery = QueryParser.ParseUpdateQuery(query)
                ?? throw new FormatException("Query parsing failure.");
            var table = InterpretTableName(updateQuery.TableName);

            var visitor = new UpdateQueryVisitor(updateQuery);

            if (visitor.WhereEvaluator is null)
                throw new FormatException("Invalid where clause.");

            foreach (var item in table.Items.Values)
            {
                lock (item.Lock)
                {
                    if (visitor.WhereEvaluator.Evaluate(item))
                        item.AcceptQueryVisitor(visitor);
                }
            }
            return visitor.UpdateCounter;
        }
        public static long InterpretDeleteQuery(string query)
        {
            var deleteQuery = QueryParser.ParseDeleteQuery(query)
                ?? throw new FormatException("Query parsing failure.");
            var table = InterpretTableName(deleteQuery.TableName);

            IWhereEvaluator whereEvaluator;
            if (WhereGenerator.Evaluators.TryGetValue(deleteQuery.TableName, out var generator))
                whereEvaluator = generator(deleteQuery.WhereConditions, deleteQuery.Conjunctions);
            else throw new FormatException($"Invalid table name ({deleteQuery.TableName}).");

            var affected = table.RemoveWhere(kvp => whereEvaluator.Evaluate(kvp.Value));
            return affected;
        }
        public static DatabaseTable<IAviationItem> InterpretTableName(string tableName)
        {
            if (!AviationDatabase.TableNames.TryGetValue(tableName, out var table) || table is null)
                throw new FormatException("Table does not exist");
            return AviationDatabase.Tables[table];
        }

        public static readonly Dictionary<string, Generator> GeneratorDictionary = new()
        {
            { TcpAcronyms.Airport,        () => new Airport(UInt64.MaxValue)},
            { TcpAcronyms.Cargo,          () => new Cargo(UInt64.MaxValue) },
            { TcpAcronyms.CargoPlane,     () => new CargoPlane(UInt64.MaxValue) },
            { TcpAcronyms.Crew,           () => new Crew(UInt64.MaxValue) },
            { TcpAcronyms.Passenger,      () => new Passenger(UInt64.MaxValue) },
            { TcpAcronyms.PassengerPlane, () => new PassengerPlane(UInt64.MaxValue) },
            { TcpAcronyms.Flight,         () => new Flight(UInt64.MaxValue, DateTime.UtcNow, DateTime.UtcNow, [], []) },
        };
    }
}