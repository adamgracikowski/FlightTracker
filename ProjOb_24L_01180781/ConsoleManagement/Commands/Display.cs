using ProjOb_24L_01180781.Database.SQL;

namespace ProjOb_24L_01180781.ConsoleManagement.Commands
{

    public class DisplayArgs : ConsoleCommandArg
    {
        public DisplayArgs(List<Task> queryTasks)
        {
            QueryTasks = queryTasks;
        }

        public List<Task> QueryTasks { get; set; }
    }
    public class Display : IConsoleCommand
    {
        public static readonly string ConsoleText = "display";

        public Display(DisplayArgs args)
        {
            Args = args;
        }

        public ulong ExecutionCounter { get; private set; }
        public bool Executed { get => ExecutionCounter > 0; }
        public DisplayArgs Args { get; set; }

        public bool Execute(string line)
        {
            ExecutionCounter++;

            var executor = new QueryExecutor();
            Args.QueryTasks.Add(Task.Run(() => { executor.ExecuteDisplayQuery(line); }));
            return true;
        }
    }
}
