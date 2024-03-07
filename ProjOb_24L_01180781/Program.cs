#define ETAP1
//#define ETAP2

using System.Globalization;

namespace ProjOb_24L_01180781
{
    public class Program
    {
        static void Main(string[] args)
        {
            var consoleManager = new ConsoleManager(args);

            // changing culture options so that the decimal separator was set to '.'
            consoleManager.SetCulture(new CultureInfo("en-US"));

#if ETAP1
            consoleManager.RunStage01();
#endif
#if ETAP2
            consoleManager.RunStage02();
#endif
        }
    }
}