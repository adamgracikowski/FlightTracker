using ProjOb_24L_01180781.DataSource.Ftr;
using ProjOb_24L_01180781.DataSource.Tcp;

namespace ProjOb_24L_01180781.AviationItems.Interfaces
{
    using DB = Database.SQL;

    public interface IAviationItem
        : IFtrItem, ITcpItem, IHasId, ILockable, DB.Visitors.IQueryable
    {
        IAviationItem Copy();
    }
}