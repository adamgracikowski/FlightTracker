using ProjOb_24L_01180781.Database.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.ConsoleManagement.Commands
{
    public class AddArgs : ConsoleCommandArg
    {
        public AddArgs(List<Task> queryTasks)
        {
            QueryTasks = queryTasks;
        }

        public List<Task> QueryTasks { get; set; }
    }
    public class Add : IConsoleCommand
    {
        public static readonly string ConsoleText = "add";

        public Add(AddArgs args)
        {
            Args = args;
        }

        public ulong ExecutionCounter { get; private set; }
        public bool Executed { get => ExecutionCounter > 0; }
        public AddArgs Args { get; set; }

        public bool Execute(string line)
        {
            ExecutionCounter++;

            var executor = new QueryExecutor();
            Args.QueryTasks.Add(Task.Run(() => { executor.ExecuteAddQuery(line); }));
            return true;
        }
    }
}
