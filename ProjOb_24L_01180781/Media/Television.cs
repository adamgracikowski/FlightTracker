using ProjOb_24L_01180781.AviationItems;

namespace ProjOb_24L_01180781.Media
{
    public class Television : IMedia
    {
        public string Name { get; private set; }
        public Television(string name)
        {
            Name = name;
        }
        public string MakeReport(CargoPlane cargoPlane)
        {
            return $"<An image of {cargoPlane.Serial} cargo plane>";
        }
        public string MakeReport(PassengerPlane passengerPlane)
        {
            return $"<An image of {passengerPlane.Serial} passenger plane>";

        }
        public string MakeReport(Airport airport)
        {
            return $"<An image of {airport.Name} airport>";
        }
    }
}
