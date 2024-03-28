using ProjOb_24L_01180781.AviationItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Media
{
    public class Radio : IMedia
    {
        public string Name { get; private set; }
        public Radio(string name)
        {
            Name = name;
        }
        public string MakeReport(CargoPlane cargoPlane)
        {
            return $"Reporting for {Name}, ladies and gentelmen, " +
                $"we are seeing the {cargoPlane.Serial} aircraft fly above us.";
        }
        public string MakeReport(PassengerPlane passengerPlane)
        {
            return $"Reporting for {Name}, ladies and gentelmen, " +
                $"we've just witnessed {passengerPlane.Serial} take off.";
        }
        public string MakeReport(Airport airport)
        {
            return $"Reporting for {Name}, ladies and gentelmen, " +
                $"we are at the {airport.Name} airport.";
        }
    }
}
