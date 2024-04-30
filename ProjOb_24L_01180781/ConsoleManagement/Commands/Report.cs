using ProjOb_24L_01180781.Database;
using ProjOb_24L_01180781.Media;

namespace ProjOb_24L_01180781.ConsoleManagement.Commands
{
    public class ReportArgs
    : ConsoleCommandArg
    {
        public List<Task> Tasks { get; set; }
        public List<IMedia> Media { get; set; }
        public ReportArgs(List<Task> tasks, List<IMedia> media)
        {
            Tasks = tasks;
            Media = media;
        }
    }
    public class Report : IConsoleCommand
    {
        public static readonly string ConsoleText = "report";
        public ulong ExecutionCounter { get; private set; }
        public bool Executed { get => ExecutionCounter > 0; }
        public ReportArgs Args { get; private set; }
        public Report(ReportArgs args)
        {
            Args = args;
            ExecutionCounter = 0;
        }
        public bool Execute(string line)
        {
            if (!line.StartsWith(ConsoleText, StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidOperationException();

            ExecutionCounter++;
            AviationDatabase.Synchronize();
            var reportable = AviationDatabase.CopyReportable().Cast<IReportable>().ToList();
            var newsGenerator = new NewsGenerator(Args.Media, reportable);
            Args.Tasks.Add(Task.Factory.StartNew(() =>
            {
                string? news;
                while ((news = newsGenerator.GenerateNextNews()) is not null)
                    Console.WriteLine(news);
            }));
            return true;
        }
    }
}
