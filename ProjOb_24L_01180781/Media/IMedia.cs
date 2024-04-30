using ProjOb_24L_01180781.AviationItems;

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
