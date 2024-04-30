using ProjOb_24L_01180781.Tools;

namespace ProjOb_24L_01180781.Database.SQL
{
    public static class QueryColumns
    {
        public static HashSet<string> AllColumns { get; set; } = new(new KeyComparer());
        public static readonly Dictionary<string, HashSet<string>> Dictionary = new(new KeyComparer())
            {
                {
                    "Airport",
                    new(["ID", "Name", "Code", "WorldPosition.Long", "WorldPosition.Lat", "AMSL", "CountryCode"],
                        new KeyComparer())
                },
                {
                    "Flight",
                    new(["ID", "Origin", "Target", "TakeOffTime", "LandingTime", "WorldPosition.Long", "WorldPosition.Lat", "AMSL", "Plane"],
                        new KeyComparer())
                },
                {
                    "PassengerPlane",
                    new(["ID", "Serial", "CountryCode", "Model", "FirstClassSize", "BusinessClassSize", "EconomyClassSize"],
                        new KeyComparer())
                },
                {
                    "CargoPlane",
                    new(["ID", "Serial", "CountryCode", "Model", "MaxLoad"],
                        new KeyComparer())
                },
                {
                    "Cargo",
                    new(["ID", "Weight", "Code", "Description"],
                        new KeyComparer())
                },
                {
                    "Passenger",
                    new(["ID", "Name", "Age", "Phone", "Email", "Class", "Miles"],
                        StringComparer.InvariantCultureIgnoreCase)
                },
                {
                    "Crew",
                    new(["ID", "Name", "Age", "Phone", "Email", "Practise", "Role"],
                        new KeyComparer())
                },
            };

        private static void BuildAllColumns()
        {
            AllColumns = new(StringComparer.InvariantCultureIgnoreCase);
            foreach (var hashSet in Dictionary.Values)
                AllColumns.UnionWith(hashSet);
        }
        static QueryColumns()
        {
            BuildAllColumns();
        }
    }
}