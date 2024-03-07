using ProjOb_24L_01180781.DataManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.ConsoleCommands
{
    public abstract class ConsoleCommandArg { }
    public class ExitArgs : ConsoleCommandArg
    {
        public List<Task> Tasks { get; set; }
        public ExitArgs(List<Task> taks)
        {
            Tasks = taks;
        }
    }
    public class PrintArgs : ConsoleCommandArg
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
}
