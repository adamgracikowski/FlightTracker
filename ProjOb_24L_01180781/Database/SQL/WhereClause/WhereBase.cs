using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.Tools;
using System.Globalization;


namespace ProjOb_24L_01180781.Database.SQL.WhereClause
{
    public abstract class WhereBase<A>
        where A : IAviationItem
    {
        public delegate bool Filter(A item, string value, string comparison);
        public delegate bool Conjunction(bool lhs, bool rhs);
        public List<WhereCondition> WhereConditions { get; set; } = [];
        public List<Conjunction> Conjunctions { get; set; } = [];
        public List<string> ConjunctionStrings { get; set; } = [];
        public List<Filter> Filters { get; set; } = [];
        public Dictionary<string, Filter> FiltersDictionary = new(new KeyComparer());
        public bool? WhereStatus { get; set; }

        public WhereBase(List<WhereCondition> whereConditions, List<string> conjunctions)
        {
            WhereConditions = whereConditions;
            ConjunctionStrings = conjunctions;
            foreach (var conjunction in conjunctions)
                Conjunctions.Add(ConjunctionsDictionary[conjunction]);
        }

        public static bool Compare<T>(T? lhs, T? rhs, string comparison)
            where T : IComparable<T>
        {
            if (lhs is null || rhs is null) return false;
            return comparison switch
            {
                ComparisonOperator.Equal => lhs.CompareTo(rhs) == 0,
                ComparisonOperator.NotEqual => lhs.CompareTo(rhs) != 0,
                ComparisonOperator.GreaterThan => lhs.CompareTo(rhs) > 0,
                ComparisonOperator.GreaterThanOrEqual => lhs.CompareTo(rhs) >= 0,
                ComparisonOperator.LessThan => lhs.CompareTo(rhs) < 0,
                ComparisonOperator.LessThanOrEqual => lhs.CompareTo(rhs) <= 0,
                _ => throw new FormatException($"Invalid comparison operator ({comparison})."),
            };
        }
        public static bool ParseCompare<T>(T? lhs, string rhs, string comparison)
            where T : IParsable<T>, IComparable<T>
        {
            if (lhs is null) return false;
            if (T.TryParse(rhs, NumberFormatInfo.InvariantInfo, out var parsed))
            {
                return Compare(lhs, parsed, comparison);
            }
            throw new FormatException($"Invalid number format ({rhs}).");
        }
        public void FetchFilters()
        {
            if (WhereConditions.Count == 0)
            {
                WhereStatus = true;
                return;
            }
            else if (WhereConditions.Count != Conjunctions.Count + 1)
                throw new FormatException("Invalid number of conjunctions (and/or).");
            foreach (var condition in WhereConditions)
            {
                if (FiltersDictionary.TryGetValue(condition.Field, out var filter))
                    Filters.Add(filter);
                else throw new FormatException($"Invalid column ({condition.Field}).");
            }
        }
        public bool EvaluateFilters(A item)
        {
            if (WhereStatus is not null)
                return WhereStatus.Value;

            bool lhs = Filters[0](item, WhereConditions[0].Value, WhereConditions[0].ComparisonOperator);

            if (Filters.Count == 1)
                return lhs;

            for (int i = 0; i < Conjunctions.Count; i++)
            {
                if (lhs && ConjunctionStrings[i] == Or)
                    return lhs;
                var rhs = Filters[i + 1](item, WhereConditions[i + 1].Value, WhereConditions[i + 1].ComparisonOperator);
                lhs = Conjunctions[i](lhs, rhs);
            }
            return lhs;
        }

        private static readonly string And = "and";
        private static readonly string Or = "or";
        private static readonly Dictionary<string, Conjunction> ConjunctionsDictionary = new(new KeyComparer())
        {
            { And, (x, y) => x && y },
            { Or,  (x, y) => x || y }
        };
    }
}