using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Exceptions
{
    public class FtrFileContext
    {
        public string Filename { get; set; }
        public UInt64 LineNumber { get; set; }

        public FtrFileContext(string filename, ulong lineNumber)
        {
            Filename = filename;
            LineNumber = lineNumber;
        }
    }
}
