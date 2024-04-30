using System.Globalization;

namespace ProjOb_24L_01180781.Tools
{
    /// <summary>
    /// Contains helper methods for parsing data out of a string object.
    /// </summary>
    public static class ParseExtender
    {
        /// <summary>
        /// This method parses an array of type T out of a string object.
        /// Elements of the parsed array are separated by one of the separator elements.
        /// Empty entries are removed.
        /// </summary>
        public static T[] ParseToArraySeparated<T>(this string data, char[] separator)
            where T : IParsable<T>
        {
            return data.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                       .Select(x => T.Parse(x, CultureInfo.InvariantCulture))
                       .ToArray();
        }
    }
}
