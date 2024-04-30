using NetworkSourceSimulator;
using ProjOb_24L_01180781.Database;
using ProjOb_24L_01180781.Exceptions;
using ProjOb_24L_01180781.Tools;
using Nss = NetworkSourceSimulator;


namespace ProjOb_24L_01180781.DataSource.Tcp
{
    /// <summary>
    /// Responsible for managing data that comes from TCP connection.
    /// </summary>
    public class TcpDataManager : DataManager
    {
        public Task RunNetworkSource(Nss.NetworkSourceSimulator networkSource)
        {
            return Task.Factory.StartNew(networkSource.Run);
        }

        [Obsolete("This method is deprecated since NetworkSourceSimulator no longer has the constructor with .ftr file as parameter")]
        public void SubscribeToNetworkSource(Nss.NetworkSourceSimulator networkSource)
        {
            var lastAcronym = TcpAcronyms.Airport;
            var lastFactory = AcronymToFactory(lastAcronym);
            var factory = lastFactory;

            networkSource.OnNewDataReady += (sender, args) =>
            {
                var source = sender as Nss.NetworkSourceSimulator;
                var messageIndex = args.MessageIndex;
                if (source != null)
                {
                    var message = source.GetMessageAt(messageIndex);
                    var acronym = ExtractAcronym(message);

                    // optimization for the case of entities with the same acronym
                    // appearing in consecutive messages
                    if (acronym != lastAcronym)
                    {
                        factory = AcronymToFactory(acronym);
                        lastFactory = factory;
                        lastAcronym = acronym;
                    }
                    var item = factory.Create(message.MessageBytes);
                    AviationDatabase.Add(item);
                }
            };
        }
        public void TakeSnapshot(string? directoryPath = null)
        {
            AviationDatabase.Synchronize();
            var snapshotDetails = SnapshotManager.TakeSnapshot(AviationDatabase.CopyState(), directoryPath);
            Console.WriteLine($"Created: {snapshotDetails.Name}");
            Console.WriteLine($"Serialized: {snapshotDetails.CollectionCount} entities");
            Console.WriteLine($"Time taken: {snapshotDetails.TimeTaken.TotalMilliseconds} ms");
        }
        protected ITcpAviationFactory AcronymToFactory(string acronym)
        {
            if (AcronymToFactoryDictionary.TryGetValue(acronym, out var factory) && factory is not null)
            {
                return factory;
            }
            else
            {
                throw new TcpFormatException($"unknown acronym (${acronym})");
            }
        }
        private static string ExtractAcronym(Message message)
        {
            int offset = 0;
            var bi = new ByteInterpreter();
            return bi.GetString(message.MessageBytes, ref offset, TcpAcronyms.Length);
        }

        /// <summary>
        /// Maps FtrAcronyms to appropriate factory objects.
        /// </summary>
        private static readonly Dictionary<string, ITcpAviationFactory> AcronymToFactoryDictionary = new()
        {
            { TcpAcronyms.Crew,           new CrewTcpFactory() },
            { TcpAcronyms.Passenger,      new PassengerTcpFactory() },
            { TcpAcronyms.Cargo,          new CargoTcpFactory() },
            { TcpAcronyms.CargoPlane,     new CargoPlaneTcpFactory() },
            { TcpAcronyms.PassengerPlane, new PassengerPlaneTcpFactory() },
            { TcpAcronyms.Airport,        new AirportTcpFactory() },
            { TcpAcronyms.Flight,         new FlightTcpFactory() }
        };
    }
}
