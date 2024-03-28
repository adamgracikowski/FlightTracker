using ProjOb_24L_01180781.AviationItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Media
{
    public class NewsPaper : IMedia
    {
        public string Name { get; private set; }
        public NewsPaper(string name)
        {
            Name = name;
        }
        public string MakeReport(CargoPlane cargoPlane)
        {
            return $"{Name} - An interview with the crew of {cargoPlane.Serial}.";
        }
        public string MakeReport(PassengerPlane passengerPlane)
        {
            return $"{Name} - Breaking news! {passengerPlane.Model} aircraft loses EASA" +
                $" fails certification after inspection of {passengerPlane.Serial}.";
        }
        public string MakeReport(Airport airport)
        {
            return $"{Name} - A report from the {airport.Name} airport, {airport.Country}.";
        }
    }
}
