using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Database.SQL.WhereClause
{
    public class WhereCondition : IEquatable<WhereCondition>
    {
        public WhereCondition(string field, string comparisonOperator, string value)
        {
            Field = field;
            ComparisonOperator = comparisonOperator;
            Value = value;
        }
        public string Field { get; set; }
        public string ComparisonOperator { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"({Field}, {ComparisonOperator}, {Value})";
        }
        public bool Equals(WhereCondition? other)
        {
            if (other == null) return false;
            return string.Compare(Field, other.Field, StringComparison.InvariantCultureIgnoreCase) == 0
                && ComparisonOperator == other.ComparisonOperator && Value == other.Value;
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as WhereCondition);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Field, ComparisonOperator, Value);
        }
    }
}
