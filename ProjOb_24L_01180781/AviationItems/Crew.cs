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
    public class Crew
        : Person, IAviationItem
    {
        public string FtrAcronym { get; } = FtrAcronyms.Crew;
        public string TcpAcronym { get; } = TcpAcronyms.Crew;

        public UInt16 Practice;
        public string? Role;
        public object Lock { get; private set; } = new();

        public Crew(UInt64 id, string? name = null, UInt64? age = null, string? phone = null,
            string? email = null, UInt16? practice = null, string? role = null)
            : base(id, name, age, phone, email)
        {
            Practice = practice ?? 0;
            Role = role;
        }
        public IAviationItem Copy()
        {
            return new Crew(Id, Name, Age, Phone, Email, Practice, Role);
        }
        public void AcceptQueryVisitor(QueryVisitor visitor)
        {
            visitor.RunQuery(this);
        }
    }
}
