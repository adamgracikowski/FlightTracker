using ProjOb_24L_01180781.DataSource;

namespace ProjOb_24L_01180781.ConsoleManagement.Dialogs
{
    public class SourceModeDialog
        : ConsoleDialog
    {
        public SourceMode PerformDialog()
        {
            Console.WriteLine("Choose data source:");
            ShowSourceModes();

            var choice = ReadWithPrompt();
            while (!_sourceModeDictionary.ContainsKey(choice))
            {
                Console.WriteLine("Invalid data source.");
                Console.WriteLine("Try again:");
                ShowSourceModes();
                choice = ReadWithPrompt();
            }
            return _sourceModeDictionary[choice];
        }
        private static void ShowSourceModes()
        {
            foreach (var key in _sourceModeDictionary.Keys)
            {
                Console.WriteLine($"> {key}");
            }
        }
        private static readonly Dictionary<string, SourceMode> _sourceModeDictionary = new()
        {
            { "ftr", SourceMode.Ftr },
            { "exit", SourceMode.None }
        };
    }
}
