using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Ftr
{
    /// <summary>
    /// Represents a set of tools for reading contents of .ftr files.
    /// </summary>
    public static class FtrReader
    {
        /// <summary>
        /// Reads the contents of the .ftr file and returns a string[] array of all lines.
        /// </summary>
        /// <param name="filename">The name of the file whose contents is to be read.</param>
        public static string[] ReadLines(string filename)
        {
            using var reader = new StreamReader(filename);
            return reader.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
