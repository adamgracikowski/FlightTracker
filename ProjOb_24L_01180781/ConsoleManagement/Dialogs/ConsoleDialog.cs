using System.Globalization;

namespace ProjOb_24L_01180781.ConsoleManagement.Dialogs
{
    public class ConsoleDialog
    {
        public static T ReadWithPrompt<T>(string? prompt = null)
            where T : IParsable<T>
        {
            if (prompt is not null)
                Console.WriteLine(prompt);
            return T.Parse(Console.ReadLine() ?? string.Empty, CultureInfo.InvariantCulture);
        }
        public static string ReadWithPrompt(string? prompt = null)
        {
            if (prompt is not null)
                Console.WriteLine(prompt);
            return Console.ReadLine() ?? string.Empty;
        }
    }
}
