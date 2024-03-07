using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Ftr
{
    public class FtrFileContext
    {
        public string Filename { get; set; }
        public ulong LineNumber { get; set; }

        public FtrFileContext(string filename, ulong lineNumber)
        {
            Filename = filename;
            LineNumber = lineNumber;
        }
    }
}
