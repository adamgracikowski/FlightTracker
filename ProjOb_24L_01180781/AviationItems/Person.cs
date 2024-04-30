using ProjOb_24L_01180781.AviationItems.Interfaces;
using System.Text.RegularExpressions;

namespace ProjOb_24L_01180781.AviationItems
{
    public abstract class Person : IContactable
    {
        public UInt64 Id { get; set; }
        public string? Name;
        public UInt64 Age;
        public string? Phone
        {
            get { return PhoneNumber; }
            set { PhoneNumber = value; }
        }
        public string? Email
        {
            get { return EmailAddress; }
            set { EmailAddress = value; }
        }

        public string? PhoneNumber;
        public string? EmailAddress;

        public Person(UInt64 id, string? name = null, UInt64? age = null, string? phone = null, string? email = null)
        {
            Id = id;
            Name = name;
            Age = age ?? 0;
            Phone = phone;
            Email = email;
        }

        public static bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, _phonePattern);
        }
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, _emailPattern);
        }

        private static readonly string _phonePattern = @"^\d{3}-\d{3}-\d{4}$";
        private static readonly string _emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.\w+)+)$";
    }
}
