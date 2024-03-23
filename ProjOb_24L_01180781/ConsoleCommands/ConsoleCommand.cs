using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.ConsoleCommands
{
    /// <summary>
    /// Represents common characteristics of all valid console commands.
    /// </summary>
    public interface IConsoleCommand
    {
        void Execute();
        UInt64 ExecutionCounter { get; }
        bool Executed { get; }
    }
    public class Exit : IConsoleCommand
    {
        public static readonly string ConsoleText = "exit";
        public UInt64 ExecutionCounter { get; private set; }
        public bool Executed { get => ExecutionCounter > 0; }

        public ExitArgs Args { get; private set; }
        public Exit(ExitArgs args)
        {
            Args = args;
            ExecutionCounter = 0;
        }
        public void Execute()
        {
            if (Args.GuiManager.IsRunnerInUse)
            {
                Console.WriteLine("To exit the program, first close the radar window.");
            }
            else
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
                ExecutionCounter++;
            }
        }
    }
    public class Print : IConsoleCommand
    {
        public static readonly string ConsoleText = "print";
        public UInt64 ExecutionCounter { get; private set; }
        public bool Executed { get => ExecutionCounter > 0; }
        public PrintArgs Args { get; private set; }
        public Print(PrintArgs args)
        {
            Args = args;
            ExecutionCounter = 0;
        }
        public void Execute()
        {
            // taking a snapshot takes place in a separate Task
            ExecutionCounter++;
            Args.Tasks.Add(Task.Factory.StartNew(() => Args.TcpManager.TakeSnapshot(Args.Directory)));
        }
    }
    public class Open : IConsoleCommand
    {
        public static readonly string ConsoleText = "open";
        public UInt64 ExecutionCounter { get; private set; }
        public bool Executed { get => ExecutionCounter > 0; }
        public OpenArgs Args { get; private set; }
        public Open(OpenArgs args)
        {
            Args = args;
            ExecutionCounter = 0;
        }
        public void Execute()
        {
            if (Args.GuiManager.IsRunnerInUse)
            {
                Console.WriteLine("The radar window is currently opened.");
            }
            else if (Args.GuiManager.IsRunnerUsed)
            {
                Console.WriteLine("The radar window has already been opened.");
                Console.WriteLine("You can open the radar window only once.");
            }
            else
            {
                ExecutionCounter++;
                Args.GuiManager.Run();
            }
        }
    }
}
