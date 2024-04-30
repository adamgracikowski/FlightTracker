using System.Text;

namespace ProjOb_24L_01180781.Database.SQL
{
    public static class QueryPresenter
    {
        public static void PrintTable(List<List<string>> data)
        {
            if (data.Count == 0)
            {
                Console.WriteLine("No data to display.");
                return;
            }

            var rows = data[0].Count;
            var widths = GetColumnWidths(data);
            var horizontalLine = GetHorizontalLine(widths);

            PrintHeader(data, widths);
            Console.WriteLine(horizontalLine);
            for (int i = 1; i < rows; i++)
                PrintRow(data, i, widths);
            Console.WriteLine();
            Console.WriteLine($"({rows - 1} rows returned)");
        }
        private static int[] GetColumnWidths(List<List<string>> data)
        {
            return data.Select(column => column.Max(e => e.Length)).ToArray();
        }
        private static string GetHorizontalLine(int[] widths)
        {
            var sb = new StringBuilder();
            foreach (var width in widths)
            {
                sb.Append(new string(Dash, width + 2));
                sb.Append(Plus);
            }
            var line = sb.ToString();
            return line;
        }
        private static void PrintHeader(List<List<string>> data, int[] widths)
        {
            var sb = new StringBuilder();

            for (int j = 0; j < data.Count; j++)
                sb.Append($"{Space}{data[j][0].PadRight(widths[j])}{Space}{Pipe}");

            var line = sb.ToString();
            Console.WriteLine(line);
        }
        private static void PrintRow(List<List<string>> data, int row, int[] widths)
        {
            var sb = new StringBuilder();

            for (int j = 0; j < data.Count; j++)
                sb.Append($"{Space}{data[j][row].PadLeft(widths[j])}{Space}{Pipe}");

            var line = sb.ToString();
            Console.WriteLine(line);
        }

        private static readonly char Dash = '-';
        private static readonly char Plus = '+';
        private static readonly char Pipe = '|';
        private static readonly char Space = ' ';
    }
}
