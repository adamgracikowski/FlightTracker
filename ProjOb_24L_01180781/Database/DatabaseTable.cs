using ProjOb_24L_01180781.AviationItems.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Database
{
    public class DatabaseTable5<T>
        where T : class?, IAviationItem
    {
        public event EventHandler<ElementAddedEventArgs<T>>? ElementAdded;
        public string Acronym { get; private set; }
        public string Name { get; private set; }
        public ConcurrentDictionary<UInt64, T> Items { get; private set; } = [];

        public DatabaseTable5(string acronym, string name)
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
                        Console.WriteLine($"Table {Name}: Item with duplicated ID ({entity.Id}) discarded.");
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
        public T? Find(UInt64 id)
        {
            if (Items.TryGetValue(id, out var item) && item is not null)
            {
                return item;
            }
            else
            {
                Console.WriteLine($"Table {Name}: Item with ID = {id} not found.");
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
        protected virtual void OnElementAdded(IEnumerable<T> addedElements)
        {
            ElementAdded?.Invoke(this, new ElementAddedEventArgs<T>(addedElements));
        }
    }
}