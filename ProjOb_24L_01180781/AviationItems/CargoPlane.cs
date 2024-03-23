using ProjOb_24L_01180781.Ftr;
using ProjOb_24L_01180781.Tcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public class CargoPlane
        : Plane, IAviationItem
    {
        public string FtrAcronym { get; } = FtrAcronyms.CargoPlane;
        public string TcpAcronym { get; } = TcpAcronyms.CargoPlane;

        public Single MaxLoad { get; private set; }

        public CargoPlane(UInt64 id, string serial, string country, string model, Single maxLoad)
            : base(id, serial, country, model)
        {
            MaxLoad = maxLoad;
        }
    }
}
