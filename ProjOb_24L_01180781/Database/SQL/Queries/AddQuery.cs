namespace ProjOb_24L_01180781.Database.SQL.Queries
{
    public class AddQuery : Query, IEquatable<AddQuery>
    {
        public AddQuery(string tableName, List<Assignment> assignments)
            : base(tableName)
        {
            Assignments = assignments;
        }
        public List<Assignment> Assignments { get; set; }

        public bool Equals(AddQuery? other)
        {
            if (other == null) return false;
            return base.Equals(other) &&
                Assignments.SequenceEqual(other.Assignments);
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as AddQuery);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
