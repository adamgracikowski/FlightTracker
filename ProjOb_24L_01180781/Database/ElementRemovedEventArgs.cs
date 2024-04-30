using ProjOb_24L_01180781.AviationItems.Interfaces;

namespace ProjOb_24L_01180781.Database
{
    public class ElementRemovedEventArgs<T> : EventArgs
    where T : class?, IAviationItem
    {
        public IEnumerable<T> RemovedElements { get; }
        public ElementRemovedEventArgs(IEnumerable<T> removedElements)
        {
            RemovedElements = removedElements;
        }
    }
}
