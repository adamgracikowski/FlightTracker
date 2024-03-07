using ProjOb_24L_01180781.ConsoleCommands;
using ProjOb_24L_01180781.DataManagers;
using ProjOb_24L_01180781.Exceptions;
using ProjOb_24L_01180781.Tools;
using System.Globalization;

using Nss = NetworkSourceSimulator;

namespace ProjOb_24L_01180781
{
    public class ConsoleManager
    {
        public string SourceFile { get; private set; }
        public int MinTcpDelay { get; private set; }
        public int MaxTcpDelay { get; private set; }

        public ConsoleManager(string[] args)
        {
            SourceFile = args.Length > 0 ? args[0] : GetSourceFileFromUser() ?? DefaultSourceFile;
            MinTcpDelay = args.Length > 1 ? int.Parse(args[1]) : DefaultMinTcpDelay;
            MaxTcpDelay = args.Length > 2 ? int.Parse(args[2]) : DefaultMaxTcpDelay;
        }
        public void SetCulture(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
        public void RunStage01()
        {
            var outputFile = $"{SourceFile}.json";
            var separator = ',';
            var ftrManager = new FtrDataManager();

            try
            {
                var entities = ftrManager.ParseFtrFile(SourceFile, separator);
                Console.WriteLine($"file {SourceFile} parsed successfully.");

                SnapshotManager.SerializeToJson(entities, outputFile);
                Console.WriteLine($"collection of objects serialized successfully to {outputFile}.");
            }
            catch (AviationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                var bytes = $"unknown exception caught: {ex.Message}";
                Console.WriteLine(bytes);
            }
        }
        public void RunStage02()
        {
            var snapshotsDirectory = SnapshotManager.TryCreateSnapshotsDirectory();
            var snapshotTasks = new List<Task>();
            var networkSource = new Nss.NetworkSourceSimulator(SourceFile, MinTcpDelay, MaxTcpDelay);
            var tcpManager = new TcpDataManager();

            var commandDictionary = new Dictionary<string, IConsoleCommand>()
            {
                { Exit.ConsoleText,  new Exit(new ExitArgs(snapshotTasks)) },
                { Print.ConsoleText, new Print(new PrintArgs(snapshotTasks, snapshotsDirectory, tcpManager)) }
            };

            Console.WriteLine("Available Commands:");
            foreach (var key in commandDictionary.Keys)
            {
                Console.WriteLine($"> {key}");
            }

            Console.WriteLine("Estabilishing Tcp connection...");

            tcpManager.SubscribeToNetworkSource(networkSource);
            var runTask = tcpManager.RunNetworkSource(networkSource);

            Console.WriteLine("Connection estabilished successfully.");
            Console.WriteLine($"Estimated speed: [{MinTcpDelay}, {MaxTcpDelay}] ms");
            string userInput;
            do
            {
                userInput = GetCommandFromUser();
                if (commandDictionary.TryGetValue(userInput.ToLowerInvariant(), out var command) && command is not null)
                {
                    command.Execute();
                }
                else
                {
                    Console.WriteLine("Invalid command.");
                }
            } while (string.Compare(userInput, Exit.ConsoleText, StringComparison.InvariantCultureIgnoreCase) != 0);
        }
        private string? GetSourceFileFromUser()
        {
            Console.WriteLine("Please provide the path to the source file: ");
            return Console.ReadLine();
        }
        private string GetCommandFromUser()
        {
            var command = Console.ReadLine() ?? string.Empty;
            return command;
        }

        private static readonly string DefaultSourceFile = "example_data.ftr";
        public static readonly int DefaultMinTcpDelay = 100;
        public static readonly int DefaultMaxTcpDelay = 200;
    }
}
