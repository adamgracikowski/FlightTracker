using ProjOb_24L_01180781.AviationItems.Interfaces;

namespace ProjOb_24L_01180781.Database
{
    public class ElementAddedEventArgs<T> : EventArgs
        where T : class?, IAviationItem
    {
        public IEnumerable<T> AddedElements { get; }
        public ElementAddedEventArgs(IEnumerable<T> removedElements)
        {
            AddedElements = removedElements;
        }
    }
}
