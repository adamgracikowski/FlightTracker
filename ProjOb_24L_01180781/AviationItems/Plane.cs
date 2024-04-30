using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public abstract class Plane
    {
        public UInt64 Id { get; set; }
        public string? Serial;
        public string? Country;
        public string? Model;
        protected Plane(UInt64 id, string? serial = null, string? country = null, string? model = null)
        {
            Id = id;
            Serial = serial;
            Country = country;
            Model = model;
        }
    }
}
