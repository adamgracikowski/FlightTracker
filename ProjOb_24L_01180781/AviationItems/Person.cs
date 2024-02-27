using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public abstract class Person
    {
        public UInt64 Id { get; private set; }
        public string Name { get; private set; }
        public UInt64 Age { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }

        public Person(UInt64 id, string name, UInt64 age, string phone, string email)
        {
            Id = id;
            Name = name;
            Age = age;
            Phone = phone;
            Email = email;
        }
    }
}
