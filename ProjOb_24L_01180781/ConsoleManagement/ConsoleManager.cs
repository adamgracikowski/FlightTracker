using ProjOb_24L_01180781.ConsoleManagement.Commands;
using ProjOb_24L_01180781.ConsoleManagement.Dialogs;
using ProjOb_24L_01180781.DataSource;
using ProjOb_24L_01180781.DataSource.Ftr;
using ProjOb_24L_01180781.DataSource.Ftre;
using ProjOb_24L_01180781.DataSource.Tcp;
using ProjOb_24L_01180781.GUI;
using ProjOb_24L_01180781.Media;
using ProjOb_24L_01180781.Tools;
using System.Globalization;
using System.Text.RegularExpressions;
using Nss = NetworkSourceSimulator;

namespace ProjOb_24L_01180781.ConsoleManagement
{
    public class ConsoleManager
    {
        public ConsoleManager(string[] args)
        {
            // parsing command line arguments
            MinTcpDelay = args.Length > 0 ? int.Parse(args[0]) : _defaultMinTcpDelay;
            MaxTcpDelay = args.Length > 1 ? int.Parse(args[1]) : _defaultMaxTcpDelay;

            SnapshotsDirectory = SnapshotManager.TryCreateSnapshotsDirectory();
            CreateMedia();
            CreateConsoleCommands();
            CreateSourceModeActions();
        }
        public void SetCulture(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
        public void Run()
        {
            SourceMode = new SourceModeDialog().PerformDialog();
            SourceModeActions[SourceMode].Invoke();
        }

        public SourceMode SourceMode { get; private set; }
        public string FtrFile { get; private set; } = string.Empty;
        public string FtreFile { get; private set; } = string.Empty;
        public int MinTcpDelay { get; init; }
        public int MaxTcpDelay { get; init; }
        public FtrDataManager FtrManager { get; init; } = new();
        public TcpDataManager TcpManager { get; init; } = new();
        public FtreDataManager FtreManager { get; init; } = new();
        public GuiManager GuiManager { get; init; } = GuiManager.GetInstance();
        public List<Task> PrintTasks { get; private set; } = [];
        public List<Task> ReportTasks { get; private set; } = [];
        public List<Task> QueryTasks { get; private set; } = [];
        public List<IMedia> Media { get; private set; } = [];
        public Dictionary<string, IConsoleCommand> CommandDictionary { get; private set; } = [];
        public Dictionary<SourceMode, Action> SourceModeActions { get; private set; } = [];
        public string SnapshotsDirectory { get; private set; }

        private void RunFtr()
        {
            var parse = CommandDictionary[Parse.ConsoleText];
            var line = ConsoleDialog.ReadWithPrompt("Please provide the path to the source file: ");
            while (!parse.Execute($"{Parse.ConsoleText} {line}"))
            {
                SourceMode = new SourceModeDialog().PerformDialog();
                if (SourceMode == SourceMode.None)
                {
                    RunNone();
                    return;
                }
                line = ConsoleDialog.ReadWithPrompt("Please provide the path to the source file: ");
            }

            FtreFile = ConsoleDialog
                .ReadWithPrompt("Please provide the path to the .ftre file: ");
            Nss.NetworkSourceSimulator networkSource;

            try
            {
                networkSource = new Nss.NetworkSourceSimulator(FtreFile, MinTcpDelay, MaxTcpDelay);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Estabilishing TCP connection failed due to:");
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine("Estabilishing TCP connection...");
            FtreManager.SubscribeToNetworkSource(networkSource);
            var runTask = FtreManager.RunNetworkSource(networkSource);

            Console.WriteLine("Connection estabilished successfully.");
            Console.WriteLine($"Estimated speed: [{MinTcpDelay}, {MaxTcpDelay}] ms");

            DialogLoop();

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
        private void RunNone()
        {
            Console.WriteLine("Exiting!");
        }
        private void DialogLoop()
        {
            var exit = CommandDictionary[Exit.ConsoleText];

            do
            {
                var dialogResult = ConsoleDialog.ReadWithPrompt();
                var match = Regex.Match(dialogResult, @"\b(?<command>\w+)\b", RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    Console.WriteLine("Invalid command.");
                    DisplayAvailableCommads();
                    continue;
                }
                if (CommandDictionary.TryGetValue(match.Groups["command"].Value, out var command) && command is not null)
                {
                    command.Execute(dialogResult);
                }
                else
                {
                    Console.WriteLine("Invalid command.");
                    DisplayAvailableCommads();
                }
            } while (exit.Executed == false);
        }
        private void CreateSourceModeActions()
        {
            SourceModeActions = new Dictionary<SourceMode, Action>
            {
                { SourceMode.Ftr, RunFtr },
                { SourceMode.None, RunNone }
            };
        }
        private void CreateMedia()
        {
            Media = new()
            {
                new Television("Telewizja Abelowa"),
                new Television("Kanał TV-tensor"),
                new Radio("Radio Kwantyfikator"),
                new Radio("Radio Shmem"),
                new NewsPaper("Gazeta Kategoryczna"),
                new NewsPaper("Dziennik Politechniczny")
            };
        }
        private void CreateConsoleCommands()
        {
            CommandDictionary = new Dictionary<string, IConsoleCommand>(new KeyComparer())
            {
                { Exit.ConsoleText,     new Exit(new ExitArgs(PrintTasks, ReportTasks, QueryTasks, GuiManager)) },
                { Print.ConsoleText,    new Print(new PrintArgs(PrintTasks, SnapshotsDirectory, TcpManager)) },
                { Open.ConsoleText,     new Open(new OpenArgs(GuiManager)) },
                { Report.ConsoleText,   new Report(new ReportArgs(ReportTasks, Media)) },
                { Parse.ConsoleText,    new Parse(new ParseArgs(FtrManager, _separator)) },
                { Add.ConsoleText,      new Add(new AddArgs(QueryTasks)) },
                { Display.ConsoleText,  new Display(new DisplayArgs(QueryTasks)) },
                { Update.ConsoleText,   new Update(new UpdateArgs(QueryTasks)) },
                { Delete.ConsoleText,   new Delete(new DeleteArgs(QueryTasks)) },
            };
        }
        private void DisplayAvailableCommads()
        {
            Console.WriteLine("Supported commands:");
            foreach (var key in CommandDictionary.Keys)
            {
                Console.WriteLine($"> {key}");
            }
        }

        [Obsolete("This method is deprecated since NetworkSourceSimulator no longer has the constructor with .ftr file as parameter")]
        private void RunTcp()
        {
            FtrFile = ConsoleDialog
                .ReadWithPrompt("Please provide the path to the source file: ");
            Nss.NetworkSourceSimulator networkSource;

            try
            {
                networkSource = new Nss.NetworkSourceSimulator(FtrFile, MinTcpDelay, MaxTcpDelay);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Estabilishing TCP connection failed due to:");
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine("Estabilishing TCP connection...");
            TcpManager.SubscribeToNetworkSource(networkSource);

            var runTask = TcpManager.RunNetworkSource(networkSource);

            Console.WriteLine("Connection estabilished successfully.");
            Console.WriteLine($"Estimated speed: [{MinTcpDelay}, {MaxTcpDelay}] ms");

            DialogLoop();

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

        private static readonly char _separator = ',';
        private static readonly int _defaultMinTcpDelay = 1;
        private static readonly int _defaultMaxTcpDelay = 2;
    }
}