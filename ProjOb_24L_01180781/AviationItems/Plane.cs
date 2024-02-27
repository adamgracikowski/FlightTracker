using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public abstract class Plane
    {
        public UInt64 Id { get; private set; }
        public string Serial { get; private set; }
        public string Country { get; private set; }
        public string Model { get; private set; }
        protected Plane(UInt64 id, string serial, string country, string model)
        {
            Id = id;
            Serial = serial;
            Country = country;
            Model = model;
        }
    }
}
