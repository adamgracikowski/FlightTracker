using ProjOb_24L_01180781.Exceptions;
using System.Globalization;

namespace ProjOb_24L_01180781
{
    public class Program
    {
        static void Main(string[] args)
        {
            // changing culture options so that the decimal separator was set to '.'
            var englishCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = englishCulture;
            CultureInfo.DefaultThreadCurrentUICulture = englishCulture;

            var inputFile = args.Length > 0 ? args[0] : (GetInputFileFromUser() ?? "example_data.ftr");
            var outputFile = $"{inputFile}.json";
            var separator = ',';

            try
            {
                var entities = AviationDataManager.ParseFtrFile(inputFile, separator);
                Console.WriteLine($"file {inputFile} parsed successfully.");

                AviationDataManager.SerializeToJson(entities, inputFile);
                Console.WriteLine($"collection of objects serialized successfully to {outputFile}.");
            }
            catch (AviationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                var message = $"unknown exception caught: {ex.Message}";
                Console.WriteLine(message);
            }
        }
        private static string? GetInputFileFromUser()
        {
            Console.WriteLine("Please provide the path to the input file: ");
            var path = Console.ReadLine();
            return path;
        }
    }
}