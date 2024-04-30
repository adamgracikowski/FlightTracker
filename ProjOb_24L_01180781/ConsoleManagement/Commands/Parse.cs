using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.Database;
using ProjOb_24L_01180781.DataSource.Ftr;
using System.Text.RegularExpressions;

namespace ProjOb_24L_01180781.ConsoleManagement.Commands
{
    public class ParseArgs
    : ConsoleCommandArg
    {
        public FtrDataManager FtrManager { get; set; }
        public char Separator { get; set; }
        public ParseArgs(FtrDataManager ftrDataManager, char separator)
        {
            FtrManager = ftrDataManager;
            Separator = separator;
        }
    }
    public class Parse : IConsoleCommand
    {
        public static readonly string ConsoleText = "parse";
        public ulong ExecutionCounter { get; private set; }
        public bool Executed { get => ExecutionCounter > 0; }
        public ParseArgs Args { get; private set; }
        public Parse(ParseArgs args)
        {
            Args = args;
            ExecutionCounter = 0;
        }
        public bool Execute(string line)
        {
            var match = Regex.Match(line, $@"^{ConsoleText}\s+(?<source>.*)$", RegexOptions.IgnoreCase);
            if (!match.Success)
                throw new InvalidOperationException();

            var source = match.Groups["source"].Value.Trim();

            List<IAviationItem> entities;
            try
            {
                Console.WriteLine("Parsing Ftr file...");
                entities = Args.FtrManager.ParseFtrFile(source, Args.Separator);
                Console.WriteLine($"File {source} parsed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Discarding changes in the database.");
                return false;
            }

            AviationDatabase.AddRange(entities);
            AviationDatabase.Synchronize();
            Console.WriteLine("Database updated.");
            return true;
        }
    }
}
