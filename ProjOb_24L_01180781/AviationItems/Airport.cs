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
        : IAviationItem, IReportable
    {
        public string FtrAcronym { get; } = FtrAcronyms.Airport;
        public string TcpAcronym { get; } = TcpAcronyms.Airport;

        public UInt64 Id { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public Location Location { get; private set; }
        public string Country { get; private set; }

        public Airport(UInt64 id, string name, string code, Location location, string country)
        {
            Id = id;
            Name = name;
            Code = code;
            Location = location;
            Country = country;
        }
        public string AcceptMediaReport(IMedia media)
        {
            return media.MakeReport(this);
        }
        public IAviationItem Copy()
        {
            return new Airport(Id, Name, Code, Location.Copy(), Country);
        }
    }
}
