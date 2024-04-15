using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.DataSource;
using ProjOb_24L_01180781.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.DataSource.Ftr
{
    /// <summary>
    /// Responsible for managing data that comes from .ftr files.
    /// </summary>
    public class FtrDataManager : DataManager
    {
        public List<IAviationItem> ParseFtrFile(string filename, char separator)
        {
            var lines = FtrReader.ReadLines(filename);
            ulong lineNumber = 0;
            var entities = new List<IAviationItem>();

            var lastAcronym = FtrAcronyms.Airport;
            var lastFactory = AcronymToFactory(lastAcronym);
            var factory = lastFactory;

            // parsing objects out of string lines
            foreach (var line in lines)
            {
                lineNumber++;

                var acronym = ExtractAcronym(line, separator);

                // optimization for the case of entities with the same acronym
                // appearing in consecutive blocks of lines
                if (acronym != lastAcronym)
                {
                    factory = AcronymToFactory(acronym, new FtrFileContext(filename, lineNumber));
                    lastFactory = factory;
                    lastAcronym = acronym;
                }

                var entityDetails = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                IAviationItem entity;
                try
                {
                    entity = factory.Create(entityDetails);
                }
                catch (Exception ex) when (ex is OverflowException || ex is FormatException)
                {
                    throw new FtrFormatException("invalid format", ex, new FtrFileContext(filename, lineNumber));
                }

                entities.Add(entity);
            }
            return entities;
        }
        protected IFtrAviationFactory AcronymToFactory(string acronym, FtrFileContext? context = null)
        {
            if (AcronymToFactoryDictionary.TryGetValue(acronym, out var factory) && factory is not null)
            {
                return factory;
            }
            else
            {
                // not finding the acronym in the dictionary means that the file was ill-formated
                throw new FtrFormatException($"unknown acronym (${acronym})", context);
            }
        }
        private static string ExtractAcronym(string line, char separator)
        {
            return line[..line.IndexOf(separator)];
        }

        /// <summary>
        /// Maps FtrAcronyms to appropriate factory objects.
        /// </summary>
        private static readonly Dictionary<string, IFtrAviationFactory> AcronymToFactoryDictionary = new()
        {
            { FtrAcronyms.Crew,           new CrewFtrFactory() },
            { FtrAcronyms.Passenger,      new PassengerFtrFactory() },
            { FtrAcronyms.Cargo,          new CargoFtrFactory() },
            { FtrAcronyms.CargoPlane,     new CargoPlaneFtrFactory() },
            { FtrAcronyms.PassengerPlane, new PassengerPlaneFtrFactory() },
            { FtrAcronyms.Airport,        new AirportFtrFactory() },
            { FtrAcronyms.Flight,         new FlightFtrFactory() }
        };
    }
}
