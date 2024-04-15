using ProjOb_24L_01180781.AviationItems.Interfaces;
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

        public UInt16 Practice { get; private set; }
        public string Role { get; private set; }
        public object Lock { get; private set; } = new();

        public Crew(UInt64 id, string name, UInt64 age, string phone, string email, UInt16 practice, string role)
            : base(id, name, age, phone, email)
        {
            Practice = practice;
            Role = role;
        }
        public IAviationItem Copy()
        {
            return new Crew(Id, Name, Age, Phone, Email, Practice, Role);
        }
    }
}
