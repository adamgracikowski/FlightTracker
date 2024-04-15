using ProjOb_24L_01180781.AviationItems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Database
{
    public class ElementAddedEventArgs<T> : EventArgs
        where T : class?, IAviationItem
    {
        public ElementAddedEventArgs(IEnumerable<T> addedElements)
        {
            AddedElements = addedElements;
        }

        public IEnumerable<T> AddedElements { get; }
    }
}
