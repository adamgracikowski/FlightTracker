using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems.Interfaces
{
    public interface IPositionable
    {
        Position GetPosition();
        void UpdatePosition(Single longitude, Single latitude, Single? amsl = null);
    }
}
