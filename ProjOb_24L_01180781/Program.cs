using ProjOb_24L_01180781.ConsoleManagement;
using System.Globalization;

namespace ProjOb_24L_01180781
{
    public class Program
    {
        static void Main(string[] args)
        {
            var consoleManager = new ConsoleManager(args);

            //changing culture options so that the decimal separator was set to '.'
            consoleManager.SetCulture(new CultureInfo("en-US"));

            try
            {
                consoleManager.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}