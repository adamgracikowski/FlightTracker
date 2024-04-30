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
    public class Passenger
        : Person, IAviationItem
    {
        public string FtrAcronym { get; } = FtrAcronyms.Passenger;
        public string TcpAcronym { get; } = TcpAcronyms.Passenger;

        public string? Class;
        public UInt64 Miles;
        public object Lock { get; private set; } = new();

        public Passenger(UInt64 id, string? name = null, UInt64? age = null, string? phone = null,
            string? email = null, string? planeClass = null, UInt64? miles = null)
            : base(id, name, age, phone, email)
        {
            Class = planeClass;
            Miles = miles ?? 0;
        }
        public IAviationItem Copy()
        {
            return new Passenger(Id, Name, Age, Phone, Email, Class, Miles);
        }
        public void AcceptQueryVisitor(QueryVisitor visitor)
        {
            visitor.RunQuery(this);
        }
    }
}
