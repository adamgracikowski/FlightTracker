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
        public UInt16 Practice { get; private set; }
        public string Role { get; private set; }

        public Crew(UInt64 id, string name, UInt64 age, string phone, string email, UInt16 practice, string role)
            : base(id, name, age, phone, email)
        {
            Practice = practice;
            Role = role;
        }
    }
}
