﻿using ProjOb_24L_01180781.GUI;

namespace ProjOb_24L_01180781.ConsoleCommands
{
    public class ExitArgs
    : ConsoleCommandArg
    {
        public List<Task> PrintTasks { get; set; }
        public List<Task> ReportTasks { get; set; }
        public GuiManager GuiManager { get; set; }
        public ExitArgs(List<Task> printTasks, List<Task> reportTasks, GuiManager guiManager)
        {
            PrintTasks = printTasks;
            ReportTasks = reportTasks;
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
                WaitForPrintTasks();
                WaitForReportTasks();
                ExecutionCounter++;
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
