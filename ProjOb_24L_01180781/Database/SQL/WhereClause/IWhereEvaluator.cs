using ProjOb_24L_01180781.AviationItems.Interfaces;

namespace ProjOb_24L_01180781.Database.SQL.WhereClause
{
    public interface IWhereEvaluator
    {
        bool Evaluate(IAviationItem item);
    }
}
