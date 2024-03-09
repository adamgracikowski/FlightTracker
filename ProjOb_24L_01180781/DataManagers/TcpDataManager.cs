using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.Tcp;
using ProjOb_24L_01180781.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkSourceSimulator;

using Nss = NetworkSourceSimulator;
using ProjOb_24L_01180781.Factories;
using ProjOb_24L_01180781.Exceptions;


namespace ProjOb_24L_01180781.DataManagers
{
    /// <summary>
    /// Responsible for managing data that comes from TCP connection.
    /// </summary>
    public class TcpDataManager : DataManager
    {
        public List<IAviationItem> Entities { get; set; }
        public TcpDataManager()
        {
            Entities = new();
            _entitiesLock = new();
        }
        public Task RunNetworkSource(Nss.NetworkSourceSimulator networkSource)
        {
            return Task.Factory.StartNew(networkSource.Run);
        }
        public void SubscribeToNetworkSource(Nss.NetworkSourceSimulator networkSource)
        {
            var lastAcronym = TcpAcronym.Airport;
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
                    var entity = factory.Create(message.MessageBytes);
                    lock (_entitiesLock)
                    {
                        Entities.Add(entity);
                    }
                }
            };
        }
        public void TakeSnapshot(string? directoryPath = null)
        {
            var snapshotDetails = SnapshotManager.TakeSnapshot(Entities, _entitiesLock, directoryPath);
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
            return bi.GetString(message.MessageBytes, ref offset, TcpAcronym.Length);
        }

        /// <summary>
        /// Maps FtrAcronym to appropriate factory objects.
        /// </summary>
        private static readonly Dictionary<string, ITcpAviationFactory> AcronymToFactoryDictionary = new()
        {
            { TcpAcronym.Crew,           new CrewTcpFactory() },
            { TcpAcronym.Passenger,      new PassengerTcpFactory() },
            { TcpAcronym.Cargo,          new CargoTcpFactory() },
            { TcpAcronym.CargoPlane,     new CargoPlaneTcpFactory() },
            { TcpAcronym.PassengerPlane, new PassengerPlaneTcpFactory() },
            { TcpAcronym.Airport,        new AirportTcpFactory() },
            { TcpAcronym.Flight,         new FlightTcpFactory() }
        };
        private object _entitiesLock;
    }
}
