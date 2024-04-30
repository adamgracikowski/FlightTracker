using ProjOb_24L_01180781.Database.SQL.WhereClause;
using ProjOb_24L_01180781.Tools;

namespace ProjOb_24L_01180781.Database.SQL.Queries
{
    public class DisplayQuery : WhereQuery, IEquatable<DisplayQuery>
    {
        public DisplayQuery(string tableName, List<string> columns, List<WhereCondition> whereConditions, List<string> conjunctions)
            : base(tableName, whereConditions, conjunctions)
        {
            Columns = columns;
        }
        public List<string> Columns { get; set; }

        public bool Equals(DisplayQuery? other)
        {
            if (other == null) return false;
            return base.Equals(other) &&
                Columns.SequenceEqual(other.Columns, new KeyComparer());
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as DisplayQuery);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
