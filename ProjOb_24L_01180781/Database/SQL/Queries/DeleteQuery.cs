using ProjOb_24L_01180781.Database.SQL.WhereClause;

namespace ProjOb_24L_01180781.Database.SQL.Queries
{
    public class DeleteQuery : WhereQuery, IEquatable<DeleteQuery>
    {
        public DeleteQuery(string tableName, List<WhereCondition> whereConditions, List<string> conjunctions)
            : base(tableName, whereConditions, conjunctions)
        { }

        public bool Equals(DeleteQuery? other)
        {
            return base.Equals(other);
        }
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
