using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapsui.Widgets;
using ProjOb_24L_01180781.Database.SQL.WhereClause;

namespace ProjOb_24L_01180781.Database.SQL.Queries
{
    public class UpdateQuery : WhereQuery, IEquatable<UpdateQuery>
    {
        public UpdateQuery(string tableName, List<Assignment> assignments, List<WhereCondition> whereConditions, List<string> conjunctions)
            : base(tableName, whereConditions, conjunctions)
        {
            Assignments = assignments;
        }
        public List<Assignment> Assignments { get; set; }

        public bool Equals(UpdateQuery? other)
        {
            if (other == null) return false;
            return base.Equals(other) &&
                Assignments.SequenceEqual(other.Assignments);
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as UpdateQuery);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
