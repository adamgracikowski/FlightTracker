namespace ProjOb_24L_01180781.AviationItems
{
    public static class AviationName
    {
        public static readonly string Airport = "Airport";
        public static readonly string Crew = "Crew";
        public static readonly string Cargo = "Cargo";
        public static readonly string CargoPlane = "CargoPlane";
        public static readonly string Passenger = "Passenger";
        public static readonly string PassengerPlane = "PassengerPlane";
        public static readonly string Flight = "Flight";

        public static readonly HashSet<string> Names
            = new([Airport, Crew, Cargo, CargoPlane, Passenger, PassengerPlane, Flight], StringComparer.InvariantCultureIgnoreCase);
    }
}
