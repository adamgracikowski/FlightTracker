using ProjOb_24L_01180781.Database.SQL.WhereClause;
using ProjOb_24L_01180781.Tools;

namespace ProjOb_24L_01180781.Database.SQL.Queries
{
    public abstract class WhereQuery : Query, IEquatable<WhereQuery>
    {
        public WhereQuery(string tableName, List<WhereCondition> whereConditions, List<string> conjunctions)
            : base(tableName)
        {
            WhereConditions = whereConditions;
            Conjunctions = conjunctions;
        }

        public List<WhereCondition> WhereConditions { get; set; }
        public List<string> Conjunctions { get; set; }

        public bool Equals(WhereQuery? other)
        {
            if (other == null) return false;
            return base.Equals(other) &&
                WhereConditions.SequenceEqual(other.WhereConditions) &&
                Conjunctions.SequenceEqual(other.Conjunctions, new KeyComparer());
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as WhereQuery);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
