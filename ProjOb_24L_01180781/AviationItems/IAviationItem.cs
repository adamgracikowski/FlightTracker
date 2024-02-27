using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    /// <summary>
    /// Represents common characteristics of all classes,
    /// which instances are stored in .ftr files.
    /// </summary>
    public interface IAviationItem
    {
        /// <summary>
        /// Uniquely identifies a class, for which details of the instance of the class, 
        /// are stored in every line of .ftr file.
        /// </summary>
        public static string Acronym { get; } = string.Empty;
    }
}
