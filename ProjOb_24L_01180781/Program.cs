//#define ETAP1
#define ETAP2

using ProjOb_24L_01180781.Exceptions;
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
            try
            {
                consoleManager.RunStage01();
            }
            catch (FtrFormatException ex)
            {
                Console.WriteLine($".ftr exception caught: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"exception caught: {ex.Message}");
            }
#endif
#if ETAP2
            try
            {
                consoleManager.RunStage02();
            }
            catch (TcpFormatException ex)
            {
                Console.WriteLine($"tcp exception caught: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"exception caught: {ex.Message}");
            }
#endif
        }
    }
}