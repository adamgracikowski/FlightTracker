using ProjOb_24L_01180781.ConsoleCommands;
using ProjOb_24L_01180781.DataManagers;
using ProjOb_24L_01180781.Exceptions;
using ProjOb_24L_01180781.GUI;
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
            // parsing command line arguments
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
        public void RunStage02and03()
        {
            var snapshotsDirectory = SnapshotManager.TryCreateSnapshotsDirectory();
            var snapshotTasks = new List<Task>();
            var networkSource = new Nss.NetworkSourceSimulator(SourceFile, MinTcpDelay, MaxTcpDelay);

            var tcpManager = new TcpDataManager();
            var guiManager = GuiManager.GetInstance();

            var commandDictionary = new Dictionary<string, IConsoleCommand>()
            {
                { Exit.ConsoleText,  new Exit(new ExitArgs(snapshotTasks, guiManager)) },
                { Print.ConsoleText, new Print(new PrintArgs(snapshotTasks, snapshotsDirectory, tcpManager)) },
                { Open.ConsoleText,  new Open(new OpenArgs(guiManager)) }
            };

            var exit = commandDictionary[Exit.ConsoleText];

            DisplayAvailableCommads(commandDictionary);

            Console.WriteLine("Estabilishing TCP connection...");

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
                    DisplayAvailableCommads(commandDictionary);
                }
            } while (exit.Executed == false);

            Console.WriteLine("Checking TCP connection for possible failures...");
            var ex = runTask.Exception;
            if (ex != null)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    Console.WriteLine(innerException.Message);
                }
            }
            else
            {
                Console.WriteLine("No failures detected.");
            }
            Console.WriteLine("Exiting!");
            Environment.Exit(0);
        }
        private static string? GetSourceFileFromUser()
        {
            Console.WriteLine("Please provide the path to the source file: ");
            return Console.ReadLine();
        }
        private static string GetCommandFromUser()
        {
            var command = Console.ReadLine() ?? string.Empty;
            return command;
        }
        private static void DisplayAvailableCommads(Dictionary<string, IConsoleCommand> commandDictionary)
        {
            Console.WriteLine("Supported commands:");
            foreach (var key in commandDictionary.Keys)
            {
                Console.WriteLine($"> {key}");
            }
        }

        private static readonly string DefaultSourceFile = "example_data.ftr";
        private static readonly int DefaultMinTcpDelay = 100;
        private static readonly int DefaultMaxTcpDelay = 200;
    }
}
