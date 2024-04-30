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
    public class PassengerPlane
        : Plane, IAviationItem, IReportable
    {
        public string FtrAcronym { get; } = FtrAcronyms.PassengerPlane;
        public string TcpAcronym { get; } = TcpAcronyms.PassengerPlane;

        public ClassSize ClassSize;
        public object Lock { get; private set; } = new();

        public PassengerPlane(UInt64 id, string? serial = null, string? country = null, string? model = null, ClassSize? classSize = null)
            : base(id, serial, country, model)
        {
            ClassSize = classSize ?? new ClassSize();
        }
        public string AcceptMediaReport(IMedia media)
        {
            return media.MakeReport(this);
        }
        public IAviationItem Copy()
        {
            return new PassengerPlane(Id, Serial, Country, Model, ClassSize.Copy());
        }
        public void AcceptQueryVisitor(QueryVisitor visitor)
        {
            visitor.RunQuery(this);
        }
    }
}
