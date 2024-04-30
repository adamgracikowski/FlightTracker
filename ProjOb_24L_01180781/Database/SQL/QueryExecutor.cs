namespace ProjOb_24L_01180781.Database.SQL
{
    public class QueryExecutor
    {
        public void ExecuteAddQuery(string query)
        {
            try
            {
                var item = QueryInterpreter.InterpretAddQuery(query);
                AviationDatabase.Add(item);
                AviationDatabase.Synchronize();
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void ExecuteDisplayQuery(string query)
        {
            try
            {
                var data = QueryInterpreter.InterpretDisplayQuery(query);
                QueryPresenter.PrintTable(data);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void ExecuteUpdateQuery(string query)
        {
            try
            {
                var affected = QueryInterpreter.InterpretUpdateQuery(query);
                Console.WriteLine($"{affected} rows affected.");
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void ExecuteDeleteQuery(string query)
        {
            try
            {
                var affected = QueryInterpreter.InterpretDeleteQuery(query);
                Console.WriteLine($"{affected} rows affected.");
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
