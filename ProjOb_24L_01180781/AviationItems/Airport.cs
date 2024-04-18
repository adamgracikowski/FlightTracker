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
    public class Airport
        : IAviationItem, IReportable, IPositionable
    {
        public string FtrAcronym { get; } = FtrAcronyms.Airport;
        public string TcpAcronym { get; } = TcpAcronyms.Airport;

        public UInt64 Id { get; set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public Position Position { get; set; }
        public string Country { get; private set; }
        public object Lock { get; private set; } = new();

        public Airport(UInt64 id, string name, string code, Position position, string country)
        {
            Id = id;
            Name = name;
            Code = code;
            Position = position;
            Country = country;
        }
        public string AcceptMediaReport(IMedia media)
        {
            return media.MakeReport(this);
        }
        public IAviationItem Copy()
        {
            return new Airport(Id, Name, Code, Position.Copy(), Country);
        }

        public void UpdatePosition(float longitude, float latitude, float? amsl = null)
        {
            Position.Update(longitude, latitude, amsl);
        }

        public Position GetPosition()
        {
            return Position.Copy();
        }
    }
}
