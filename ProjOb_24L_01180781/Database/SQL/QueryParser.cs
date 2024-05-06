namespace ProjOb_24L_01180781.Database.SQL
{
    using ProjOb_24L_01180781.ConsoleManagement.Commands;
    using ProjOb_24L_01180781.Database.SQL.Queries;
    using ProjOb_24L_01180781.Database.SQL.WhereClause;
    using System.Text.RegularExpressions;

    public static class QueryParser
    {
        public static DeleteQuery? ParseDeleteQuery(string query)
        {
            var match = Regex.Match(query, QueryRegex.DeleteBase, RegexOptions.IgnoreCase);
            if (!match.Success)
                return null;

            var tableName = match.Groups[2].Value;
            var hasWhereClause = match.Groups[3].Success;
            var conditions = match.Groups[4].Value;

            if (!hasWhereClause)
            {
                return new DeleteQuery(tableName, [], []);
            }
            else if (string.IsNullOrEmpty(conditions))
                return null;

            if (!ParseWhereConditions(conditions, out var whereConditions, out var conjunctions) ||
                whereConditions is null || conjunctions is null)
                return null;

            return new DeleteQuery(tableName, whereConditions, conjunctions);
        }
        public static DisplayQuery? ParseDisplayQuery(string query)
        {
            var match = Regex.Match(query, QueryRegex.DisplayBase, RegexOptions.IgnoreCase);
            if (!match.Success)
                return null;

            var tableName = match.Groups[4].Value;
            var columns = match.Groups[2].Value;

            if (!ParseDisplayColumns(columns, out var columnsList) || columnsList is null)
                return null;

            var hasWhereClause = match.Groups[5].Success;
            var conditions = match.Groups[6].Value;

            if (!hasWhereClause)
            {
                return new DisplayQuery(tableName, columnsList, [], []);
            }
            else if (string.IsNullOrEmpty(conditions))
                return null;

            if (!ParseWhereConditions(conditions, out var whereConditions, out var conjunctions) ||
                whereConditions is null || conjunctions is null)
                return null;

            return new DisplayQuery(tableName, columnsList, whereConditions, conjunctions);
        }
        public static AddQuery? ParseAddQuery(string query)
        {
            var match = Regex.Match(query, QueryRegex.AddBase, RegexOptions.IgnoreCase);
            if (!match.Success)
                return null;

            var tableName = match.Groups[2].Value;
            var assignments = match.Groups[4].Value;

            if (!ParseAssignments(assignments, out var assignmentsList) || assignmentsList is null)
                return null;

            return new AddQuery(tableName, assignmentsList);
        }
        public static UpdateQuery? ParseUpdateQuery(string query)
        {
            var match = Regex.Match(query, QueryRegex.UpdateBase, RegexOptions.IgnoreCase);
            if (!match.Success)
                return null;

            var tableName = match.Groups[2].Value;
            var assignments = match.Groups[4].Value;

            if (!ParseAssignments(assignments, out var assignmentsList) || assignmentsList is null)
                return null;

            var hasWhereClause = match.Groups[5].Success;
            var conditions = match.Groups[6].Value;

            if (!hasWhereClause)
            {
                return new UpdateQuery(tableName, assignmentsList, [], []);
            }
            else if (string.IsNullOrEmpty(conditions))
                return null;

            if (!ParseWhereConditions(conditions, out var whereConditions, out var conjunctions) ||
                whereConditions is null || conjunctions is null)
                return null;

            return new UpdateQuery(tableName, assignmentsList, whereConditions, conjunctions);
        }
        private static bool ParseDisplayColumns(string query, out List<string> columns)
        {
            columns = [];
            var match = Regex.Match(query, QueryRegex.Columns, RegexOptions.IgnoreCase);
            if (!match.Success) return false;

            var star = match.Groups[1].Value;

            if (!string.IsNullOrEmpty(star))
            {
                columns = [star];
            }
            else
            {
                columns = match.Groups[2].Captures.Select(c => c.Value).ToList();
                var column = match.Groups[3].Value;
                columns.Add(column);
            }

            return true;
        }
        private static bool ParseWhereConditions(string conditions,
            out List<WhereCondition> whereConditions, out List<string> conjunctions)
        {
            whereConditions = [];
            conjunctions = [];

            var match = Regex.Match(conditions, QueryRegex.Conditions, RegexOptions.IgnoreCase);
            if (!match.Success)
                return false;

            var field = match.Groups[2].Value;
            var comparisonOperator = match.Groups[3].Value;
            var value = match.Groups[4].Value;

            conjunctions = match.Groups[5].Captures.Select(c => c.Value).ToList();
            var fields = match.Groups[6].Captures.Select(c => c.Value).ToList();
            var comparisonOperators = match.Groups[7].Captures.Select(c => c.Value).ToList();
            var values = match.Groups[8].Captures.Select(c => c.Value).ToList();

            if (!string.IsNullOrEmpty(field)) fields.Insert(0, field);
            if (!string.IsNullOrEmpty(comparisonOperator)) comparisonOperators.Insert(0, comparisonOperator);
            if (!string.IsNullOrEmpty(value)) values.Insert(0, value);

            values = values.Select(value => value.Trim('\'')).ToList();

            for (int i = 0; i < fields.Count; i++)
                whereConditions.Add(new(fields[i], comparisonOperators[i], values[i]));

            return true;
        }
        private static bool ParseAssignments(string query, out List<Assignment> assignments)
        {
            assignments = [];
            var match = Regex.Match(query, QueryRegex.Assignments, RegexOptions.IgnoreCase);
            if (!match.Success) return false;

            var field = match.Groups[2].Value;
            var value = match.Groups[3].Value;
            var fields = match.Groups[4].Captures.Select(c => c.Value).ToList();
            var values = match.Groups[5].Captures.Select(c => c.Value).ToList();
            if (!string.IsNullOrEmpty(field)) fields.Insert(0, field);
            if (!string.IsNullOrEmpty(value)) values.Insert(0, value);

            values = values.Select(value => value.Trim('\'')).ToList();

            for (int i = 0; i < fields.Count; i++)
                assignments.Add(new Assignment(fields[i], values[i]));
            return true;
        }
    }

}
