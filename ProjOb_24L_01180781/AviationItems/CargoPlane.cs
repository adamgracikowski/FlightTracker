using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.DataSource.Ftr;
using ProjOb_24L_01180781.DataSource.Tcp;
using ProjOb_24L_01180781.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public class CargoPlane
        : Plane, IAviationItem, IReportable
    {
        public string FtrAcronym { get; } = FtrAcronyms.CargoPlane;
        public string TcpAcronym { get; } = TcpAcronyms.CargoPlane;

        public Single MaxLoad { get; private set; }

        public CargoPlane(UInt64 id, string serial, string country, string model, Single maxLoad)
            : base(id, serial, country, model)
        {
            MaxLoad = maxLoad;
        }
        public string AcceptMediaReport(IMedia media)
        {
            return media.MakeReport(this);
        }
        public IAviationItem Copy()
        {
            return new CargoPlane(Id, Serial, Country, Model, MaxLoad);
        }
    }
}
