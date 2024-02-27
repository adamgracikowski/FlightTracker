using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public struct ClassSize
    {
        public UInt16 First { get; set; }
        public UInt16 Business { get; set; }
        public UInt16 Economy { get; set; }
        public ClassSize(UInt16 first, UInt16 business, UInt16 economy)
        {
            First = first;
            Business = business;
            Economy = economy;
        }
    }
}
