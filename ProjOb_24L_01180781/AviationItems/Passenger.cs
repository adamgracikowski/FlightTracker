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
    public class Passenger
        : Person, IAviationItem
    {
        public string FtrAcronym { get; } = FtrAcronyms.Passenger;
        public string TcpAcronym { get; } = TcpAcronyms.Passenger;

        public string Class { get; private set; }
        public UInt64 Miles { get; private set; }

        public Passenger(UInt64 id, string name, UInt64 age, string phone, string email, string planeClass, UInt64 miles)
            : base(id, name, age, phone, email)
        {
            Class = planeClass;
            Miles = miles;
        }
        public IAviationItem Copy()
        {
            return new Passenger(Id, Name, Age, Phone, Email, Class, Miles);
        }
    }
}
