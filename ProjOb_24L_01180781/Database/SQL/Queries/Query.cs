using ProjOb_24L_01180781.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Database.SQL.Queries
{
    public abstract class Query : IEquatable<Query>
    {
        public Query(string tableName)
        {
            TableName = tableName;
        }

        public string TableName { get; set; }

        public bool Equals(Query? other)
        {
            if (other == null) return false;
            return string.Compare(TableName, other.TableName, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Query);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
