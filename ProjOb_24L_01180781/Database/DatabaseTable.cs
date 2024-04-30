using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Database
{
    public class DatabaseTable<T>
        where T : class?, IAviationItem
    {
        public event EventHandler<ElementAddedEventArgs<T>>? ElementAdded;
        public event EventHandler<ElementRemovedEventArgs<T>>? ElementRemoved;
        public string Acronym { get; private set; }
        public string Name { get; private set; }
        public ConcurrentDictionary<UInt64, T> Items { get; private set; } = [];

        public DatabaseTable(string acronym, string name)
        {
            Acronym = acronym;
            Name = name;
        }
        public void AddRange(IEnumerable<T> entities)
        {
            var added = new List<T>();
            foreach (var entity in entities)
            {
                if (!Items.TryAdd(entity.Id, entity))
                {
                    lock (entity.Lock)
                        Console.WriteLine($"{Name}: Element with ID = {entity.Id} already exists.");
                }
                else added.Add(entity);
            }
            if (added.Count > 0)
                OnElementAdded(added);
        }
        public void AddRange(IEnumerable<IAviationItem> entities)
        {
            AddRange(entities.Where(item => item.TcpAcronym == Acronym).Cast<T>());
        }
        public T? Find(UInt64? id)
        {
            if (id is null) return null;
            if (Items.TryGetValue(id.Value, out var item) && item is not null)
            {
                return item;
            }
            else
            {
                Console.WriteLine($"{Name}: Element with ID = {id} not found.");
                return null;
            }
        }
        public void CopyTo(List<IAviationItem> destination)
        {
            destination.AddRange(Items.Select(item =>
            {
                lock (item.Value.Lock)
                    return item.Value.Copy();
            }));
        }
        public long RemoveWhere(Predicate<KeyValuePair<UInt64, T>> predicate)
        {
            var (yes, no) = Items.PartitionBy(kvp => { lock (kvp.Value.Lock) return predicate(kvp); });
            Items = new(no);
            if (yes.Count != 0) OnElementRemoved(yes.Select(kvp => kvp.Value));
            return yes.Count;
        }

        protected virtual void OnElementAdded(IEnumerable<T> addedElements)
        {
            ElementAdded?.Invoke(this, new ElementAddedEventArgs<T>(addedElements));
        }
        protected virtual void OnElementRemoved(IEnumerable<T> addedElements)
        {
            ElementRemoved?.Invoke(this, new ElementRemovedEventArgs<T>(addedElements));
        }
    }
}