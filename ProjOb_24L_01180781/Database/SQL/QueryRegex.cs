using ProjOb_24L_01180781.ConsoleManagement.Commands;

namespace ProjOb_24L_01180781.Database.SQL
{
    public static class QueryRegex
    {
        public static readonly string NumberValue = @"-?\d+(?:\.\d+)?";
        public static readonly string StringValue = @"'[^']*'";
        public static readonly string Operators = @">|<|>=|<=|=|!=";
        public static readonly string Conjunctions = @"and|or";
        public static readonly string Name = @"[a-z]+";
        public static readonly string NameWithDot = @$"[a-z]+\.?[a-z]*";

        public static readonly string DeleteBase = $@"^({Delete.ConsoleText})\s+({Name})\s*(where)?\s*(?(3)(.*))$";
        public static readonly string Condition = $@"({NameWithDot})\s*({Operators})\s*({NumberValue}|{StringValue})";
        public static readonly string Conditions = $@"^({Condition})(?(1)\s*({Conjunctions})\s*{Condition}\s*)*$";

        public static readonly string DisplayBase = $@"^({Display.ConsoleText})\s+(.*)\s+(from)\s+({Name})\s*(where)?\s*(?(5)(.*))$";
        public static readonly string Columns = $@"^(?:(\*)|(?:({NameWithDot})\s*,\s*)*({NameWithDot})\s*)$";

        public static readonly string AddBase = $@"^({Add.ConsoleText})\s+({Name})\s+(new)\s*\((.*)\)\s*";

        public static readonly string Assignment = $@"({NameWithDot})\s*=\s*({NumberValue}|{StringValue})";
        public static readonly string Assignments = $@"^({Assignment})\s*(?(1)(?:\s*,\s*{Assignment})*)\s*$";

        public static readonly string UpdateBase = $@"^({Update.ConsoleText})\s+({Name})\s+(set)\s*\((.*)\)\s*(where)?\s*(?(5)(.*))$";
    }
}
