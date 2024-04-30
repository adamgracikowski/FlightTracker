using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.Database.SQL.Visitors;
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

        public Single MaxLoad;
        public object Lock { get; private set; } = new();

        public CargoPlane(UInt64 id, string? serial = null, string? country = null, string? model = null, Single? maxLoad = null)
            : base(id, serial, country, model)
        {
            MaxLoad = maxLoad ?? 0;
        }
        public string AcceptMediaReport(IMedia media)
        {
            return media.MakeReport(this);
        }
        public IAviationItem Copy()
        {
            return new CargoPlane(Id, Serial, Country, Model, MaxLoad);
        }
        public void AcceptQueryVisitor(QueryVisitor visitor)
        {
            visitor.RunQuery(this);
        }
    }
}
