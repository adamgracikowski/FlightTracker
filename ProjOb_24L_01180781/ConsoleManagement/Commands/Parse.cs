using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.ConsoleManagement.Dialogs;
using ProjOb_24L_01180781.Database;
using ProjOb_24L_01180781.DataSource.Ftr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void Execute()
        {
            var source = ConsoleDialog
                .ReadWithPrompt("Please provide the path to the source file: ");

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
                return;
            }

            AviationDatabase.AddRange(entities);
            AviationDatabase.Synchronize();
            Console.WriteLine("Database updated.");
        }
    }
}
