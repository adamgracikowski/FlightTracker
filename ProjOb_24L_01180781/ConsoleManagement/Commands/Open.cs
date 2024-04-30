using ProjOb_24L_01180781.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.ConsoleManagement.Commands
{
    public class OpenArgs
    : ConsoleCommandArg
    {
        public GuiManager GuiManager { get; set; }
        public OpenArgs(GuiManager guiManager)
        {
            GuiManager = guiManager;
        }
    }
    public class Open : IConsoleCommand
    {
        public static readonly string ConsoleText = "open";
        public ulong ExecutionCounter { get; private set; }
        public bool Executed { get => ExecutionCounter > 0; }
        public OpenArgs Args { get; private set; }
        public Open(OpenArgs args)
        {
            Args = args;
            ExecutionCounter = 0;
        }
        public bool Execute(string line)
        {
            if (!line.StartsWith(ConsoleText, StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidOperationException();

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
                return true;
            }
            return false;
        }
    }
}
