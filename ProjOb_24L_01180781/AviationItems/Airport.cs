using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public class Airport
        : IAviationItem
    {
        public UInt64 Id { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public Location Location { get; private set; }
        public string Country { get; private set; }

        public Airport(UInt64 id, string name, string code, Location location, string country)
        {
            Id = id;
            Name = name;
            Code = code;
            Location = location;
            Country = country;
        }
    }
}
