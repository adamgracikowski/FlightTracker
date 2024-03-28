using ProjOb_24L_01180781.AviationItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Media
{
    public interface IMedia
    {
        string Name { get; }
        string MakeReport(CargoPlane cargoPlane);
        string MakeReport(PassengerPlane passengerPlane);
        string MakeReport(Airport airport);
    }
}
