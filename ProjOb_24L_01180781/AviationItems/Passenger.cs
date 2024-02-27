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
        public static string Acronym => _acronym;
        public string Class { get; private set; }
        public UInt64 Miles { get; private set; }

        public Passenger(UInt64 id, string name, UInt64 age, string phone, string email, string planeClass, UInt64 miles)
            : base(id, name, age, phone, email)
        {
            Class = planeClass;
            Miles = miles;
        }

        private static readonly string _acronym = "P";
    }
}
