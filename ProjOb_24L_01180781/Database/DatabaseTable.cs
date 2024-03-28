using ProjOb_24L_01180781.AviationItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Database
{
    public class DatabaseTable<T>
        where T : class?, IAviationItem
    {
        public readonly object Lock = new();
        public string Acronym { get; private set; }
        public string Name { get; private set; }
        public List<T> Items { get; private set; } = [];
        public Dictionary<UInt64, T> Index { get; private set; } = [];
        public DatabaseTable(string acronym, string name)
        {
            Acronym = acronym;
            Name = name;
        }
        public void AddRange(IEnumerable<T> entities)
        {
            lock (Lock)
            {
                foreach (var entity in entities)
                {
                    if (!Index.TryAdd(entity.Id, entity))
                    {
                        Console.WriteLine($"Table {Name}: Item with duplicated ID ({entity.Id}) discarded.");
                    }
                    else
                    {
                        Items.Add(entity);
                    }
                }
            }
        }
        public void AddRange(IEnumerable<IAviationItem> entities)
        {
            AddRange(entities.Where(item => item.TcpAcronym == Acronym).Cast<T>());
        }
        public T? Find(UInt64 id)
        {
            lock (Lock)
            {
                if (Index.TryGetValue(id, out var item) && item is not null)
                {
                    return item;
                }
                else
                {
                    Console.WriteLine($"Table {Name}: Item with ID = {id} not found.");
                    return null;
                }
            }
        }
        public void CopyTo(List<IAviationItem> destination)
        {
            lock (Lock)
            {
                destination.AddRange(Items.Select(item => item.Copy()));
            }
        }
    }
}
