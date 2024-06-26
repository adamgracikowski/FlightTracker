﻿using ProjOb_24L_01180781.DataSource.Tcp;

namespace ProjOb_24L_01180781.ConsoleManagement.Commands
{
    public class PrintArgs
    : ConsoleCommandArg
    {
        public List<Task> Tasks { get; set; }
        public string Directory { get; set; }
        public TcpDataManager TcpManager { get; set; }
        public PrintArgs(List<Task> tasks, string directory, TcpDataManager tcpManager)
        {
            Tasks = tasks;
            Directory = directory;
            TcpManager = tcpManager;
        }
    }
    public class Print : IConsoleCommand
    {
        public static readonly string ConsoleText = "print";
        public ulong ExecutionCounter { get; private set; }
        public bool Executed { get => ExecutionCounter > 0; }
        public PrintArgs Args { get; private set; }
        public Print(PrintArgs args)
        {
            Args = args;
            ExecutionCounter = 0;
        }
        public bool Execute(string line)
        {
            if (!line.StartsWith(ConsoleText, StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidOperationException();

            // taking a snapshot takes place in a separate Task
            ExecutionCounter++;
            Args.Tasks.Add(Task.Factory.StartNew(() => Args.TcpManager.TakeSnapshot(Args.Directory)));
            return true;
        }
    }
}
