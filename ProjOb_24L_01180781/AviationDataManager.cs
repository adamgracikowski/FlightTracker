using Newtonsoft.Json;
using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.Exceptions;
using ProjOb_24L_01180781.Factories;
using ProjOb_24L_01180781.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781
{
    /// <summary>
    /// Responsible for the two main operations specified for the first phase of the OOD project, 
    /// that is: parsing .ftr files and serializing data to the .json files.
    /// </summary>
    public static class AviationDataManager
    {
        public static List<IAviationItem> ParseFtrFile(string filename, char separator)
        {
            var lines = FtrReader.ReadLines(filename);
            UInt64 lineNumber = 0;
            var entities = new List<IAviationItem>();

            var lastAcronym = Airport.Acronym;
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
        public static void SerializeToJson(List<IAviationItem> entities, string inputFile, string? outputFile = null)
        {
            outputFile ??= $"{inputFile}.json";

            using var writer = new StreamWriter(outputFile);

            // serializing each entity from the collection
            foreach (var entity in entities)
            {
                var json = JsonConvert.SerializeObject(entity);
                writer.WriteLine(json);
            }
        }

        private static string ExtractAcronym(string line, char separator)
        {
            return line[..line.IndexOf(separator)];
        }
        private static IAviationFactory AcronymToFactory(string acronym, FtrFileContext? context = null)
        {
            if (AcronymToFactoryDictionary.TryGetValue(acronym, out var factory) && factory is not null)
            {
                return factory;
            }
            else
            {
                // not finding the acronym in the dictionary means that the file was ill-formated
                throw new FtrFormatException($"unknown acronym ({acronym})", context);
            }
        }

        /// <summary>
        /// Maps acronyms of particular classes to appropriate factory objects.
        /// </summary>
        private static readonly Dictionary<string, IAviationFactory> AcronymToFactoryDictionary = new()
        {
            { Crew.Acronym,             new CrewFactory() },
            { Passenger.Acronym,        new PassengerFactory() },
            { Cargo.Acronym,            new CargoFactory() },
            { CargoPlane.Acronym,       new CargoPlaneFactory() },
            { PassengerPlane.Acronym,   new PassengerPlaneFactory() },
            { Airport.Acronym,          new AirportFactory() },
            { Flight.Acronym,           new FlightFactory() }
        };
    }
}
