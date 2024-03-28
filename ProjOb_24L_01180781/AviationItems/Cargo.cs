using ProjOb_24L_01180781.DataSource.Ftr;
using ProjOb_24L_01180781.DataSource.Tcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public class Cargo
        : IAviationItem
    {
        public string FtrAcronym { get; } = FtrAcronyms.Cargo;
        public string TcpAcronym { get; } = TcpAcronyms.Cargo;

        public UInt64 Id { get; private set; }
        public Single Weight { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }

        public Cargo(UInt64 id, Single weight, string code, string description)
        {
            Id = id;
            Weight = weight;
            Code = code;
            Description = description;
        }
        public IAviationItem Copy()
        {
            return new Cargo(Id, Weight, Code, Description);
        }
    }
}
