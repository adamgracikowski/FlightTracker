using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Database.SQL
{
    public class Assignment : IEquatable<Assignment>
    {
        public Assignment(string field, string value)
        {
            Field = field;
            Value = value;
        }
        public string Field { get; set; }
        public string Value { get; set; }
        public override string ToString()
        {
            return $"({Field}, {Value})";
        }
        public bool Equals(Assignment? other)
        {
            if (other is null) return false;
            return string.Compare(Field, other.Field, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                Value == other.Value;
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Assignment);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Field, Value);
        }
    }

}
