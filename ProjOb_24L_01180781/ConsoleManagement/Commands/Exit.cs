using ProjOb_24L_01180781.DataSource.Ftre;
using ProjOb_24L_01180781.GUI;

namespace ProjOb_24L_01180781.ConsoleManagement.Commands
{
    public class ExitArgs
    : ConsoleCommandArg
    {
        public List<Task> PrintTasks { get; set; }
        public List<Task> ReportTasks { get; set; }
        public List<Task> QueryTasks { get; set; }
        public GuiManager GuiManager { get; set; }
        public ExitArgs(List<Task> printTasks, List<Task> reportTasks, List<Task> queryTasks, GuiManager guiManager)
        {
            PrintTasks = printTasks;
            ReportTasks = reportTasks;
            QueryTasks = queryTasks;
            GuiManager = guiManager;
        }
    }
    public class Exit : IConsoleCommand
    {
        public static readonly string ConsoleText = "exit";
        public ulong ExecutionCounter { get; private set; }
        public bool Executed { get => ExecutionCounter > 0; }

        public ExitArgs Args { get; private set; }
        public Exit(ExitArgs args)
        {
            Args = args;
        }
        public bool Execute(string line)
        {
            if (!line.StartsWith(ConsoleText, StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidOperationException();

            if (Args.GuiManager.IsRunnerInUse)
            {
                Console.WriteLine("To exit the program, first close the radar window.");
                return false;
            }
            else
            {
                WaitForPrintTasks();
                WaitForReportTasks();
                WaitForLogTasks();
                WaitForQueryTasks();

                ExecutionCounter++;
                return true;
            }
        }

        private void WaitForPrintTasks()
        {
            Console.WriteLine("Waiting for all the snapshots to finish...");
            try
            {
                Task.WaitAll([.. Args.PrintTasks]);
            }
            catch (AggregateException ex)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    Console.WriteLine(innerException.Message);
                }
            }
        }
        private void WaitForQueryTasks()
        {
            Console.WriteLine("Waiting for all the queries to finish...");
            try
            {
                Task.WaitAll([.. Args.QueryTasks]);
            }
            catch (AggregateException ex)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    Console.WriteLine(innerException.Message);
                }
            }
        }
        private static void WaitForLogTasks()
        {
            Console.WriteLine("Waiting for all the logs to finish...");
            try
            {
                LogManager.GetInstance().Dispose();
            }
            catch (AggregateException ex)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    Console.WriteLine(innerException.Message);
                }
            }
        }
        private void WaitForReportTasks()
        {
            Console.WriteLine("Waiting for all the reports to finish...");
            try
            {
                Task.WaitAll([.. Args.ReportTasks]);
            }
            catch (AggregateException ex)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    Console.WriteLine(innerException.Message);
                }
            }
        }
    }
}
