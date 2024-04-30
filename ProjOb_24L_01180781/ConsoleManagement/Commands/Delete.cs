using ProjOb_24L_01180781.Database.SQL;

namespace ProjOb_24L_01180781.ConsoleManagement.Commands
{
    public class DeleteArgs : ConsoleCommandArg
    {
        public DeleteArgs(List<Task> queryTasks)
        {
            QueryTasks = queryTasks;
        }

        public List<Task> QueryTasks { get; set; }
    }
    public class Delete : IConsoleCommand
    {
        public static readonly string ConsoleText = "delete";

        public Delete(DeleteArgs args)
        {
            Args = args;
        }

        public ulong ExecutionCounter { get; private set; }
        public bool Executed { get => ExecutionCounter > 0; }
        public DeleteArgs Args { get; set; }

        public bool Execute(string line)
        {
            ExecutionCounter++;

            var executor = new QueryExecutor();
            Args.QueryTasks.Add(Task.Run(() => { executor.ExecuteDeleteQuery(line); }));
            return true;
        }
    }
}
