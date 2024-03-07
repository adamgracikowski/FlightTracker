using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.ConsoleCommands
{
    public interface IConsoleCommand
    {
        void Execute();
    }
    public class Exit : IConsoleCommand
    {
        public static readonly string ConsoleText = "exit";
        public ExitArgs Args { get; private set; }
        public Exit(ExitArgs args)
        {
            Args = args;
        }
        public void Execute()
        {
            Console.WriteLine("Waiting for all the snapshots to finish...");
            try
            {
                Task.WaitAll([.. Args.Tasks]);
            }
            catch (AggregateException ex)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    Console.WriteLine(innerException.Message);
                }
            }
            Console.WriteLine("Exiting!");
            Environment.Exit(0);
        }
    }
    public class Print : IConsoleCommand
    {
        public static readonly string ConsoleText = "print";
        public PrintArgs Args { get; private set; }
        public Print(PrintArgs args)
        {
            Args = args;
        }
        public void Execute()
        {
            Args.Tasks.Add(Task.Factory.StartNew(() => Args.TcpManager.TakeSnapshot(Args.Directory)));
        }
    }
}
