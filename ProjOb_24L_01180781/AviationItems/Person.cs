using ProjOb_24L_01180781.AviationItems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public abstract class Person : IContactable
    {
        public UInt64 Id { get; set; }
        public string Name { get; private set; }
        public UInt64 Age { get; private set; }
        public string Phone
        {
            get => _phone;
            set
            {
                if (Regex.IsMatch(value, _phonePattern))
                    _phone = value;
                else throw new InvalidDataException("invalid phone number");
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                if (Regex.IsMatch(value, _emailPattern))
                    _email = value;
                else throw new InvalidDataException("invalid email address");
            }
        }

        public Person(UInt64 id, string name, UInt64 age, string phone, string email)
        {
            Id = id;
            Name = name;
            Age = age;
            Phone = phone;
            Email = email;
        }

        private string _phone = string.Empty;
        private string _email = string.Empty;
        private static readonly string _phonePattern = @"^\d{3}-\d{3}-\d{4}$";
        private static readonly string _emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.\w+)+)$";
    }
}
