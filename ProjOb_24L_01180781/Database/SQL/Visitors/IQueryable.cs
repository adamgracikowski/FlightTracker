namespace ProjOb_24L_01180781.Database.SQL.Visitors
{
    public interface IQueryable
    {
        void AcceptQueryVisitor(QueryVisitor visitor);
    }
}
