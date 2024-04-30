using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.Database.SQL.Visitors;
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

        public UInt64 Id { get; set; }
        public Single Weight;
        public string? Code;
        public string? Description;
        public object Lock { get; private set; } = new();

        public Cargo(UInt64 id, Single? weight = null, string? code = null, string? description = null)
        {
            Id = id;
            Weight = weight ?? 0;
            Code = code;
            Description = description;
        }
        public IAviationItem Copy()
        {
            return new Cargo(Id, Weight, Code, Description);
        }
        public void AcceptQueryVisitor(QueryVisitor visitor)
        {
            visitor.RunQuery(this);
        }
    }
}
