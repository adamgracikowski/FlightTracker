using ProjOb_24L_01180781.DataSource.Ftr;
using ProjOb_24L_01180781.DataSource.Tcp;

namespace ProjOb_24L_01180781.AviationItems.Interfaces
{
    public interface IAviationItem
        : IFtrItem, ITcpItem, IHasId
    {
        IAviationItem Copy();
    }
}
