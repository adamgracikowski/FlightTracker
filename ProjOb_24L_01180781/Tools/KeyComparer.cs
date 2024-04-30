using System.Diagnostics.CodeAnalysis;

namespace ProjOb_24L_01180781.Tools
{
    public class KeyComparer : IEqualityComparer<string>
    {
        public bool Equals(string? x, string? y)
        {
            return string.Equals(x, y, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode([DisallowNull] string obj)
        {
            return obj.ToUpper().GetHashCode();
        }
    }
}