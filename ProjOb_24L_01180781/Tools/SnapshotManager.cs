using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Tools
{
    public static class SnapshotManager
    {
        public static SnapshotDetails TakeSnapshot<T>(IEnumerable<T> collection, string? directoryPath = null)
        {
            string name, fullname;
            int collectionCount;
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            name = GetSnapshotFileName();
            fullname = (directoryPath != null) ? Path.Combine(directoryPath, name) : name;
            collectionCount = SerializeToJson(collection, fullname);
            stopwatch.Stop();

            return new SnapshotDetails(name, collectionCount, stopwatch.Elapsed);
        }
        public static int SerializeToJson<T>(IEnumerable<T> collection, string filename)
        {
            var counter = 0;
            using var writer = new StreamWriter(filename);

            // serializing each entity from the collection
            foreach (var element in collection)
            {
                var json = JsonConvert.SerializeObject(element);
                writer.WriteLine(json);
                counter++;
            }
            return counter;
        }
        public static string TryCreateSnapshotsDirectory()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var snapshotsDirectory = Path.Combine(baseDirectory, "snapshots");
            if (!Directory.Exists(snapshotsDirectory))
            {
                Directory.CreateDirectory(snapshotsDirectory);
                Console.WriteLine($"{Path.DirectorySeparatorChar}snapshots created.");
            }
            return snapshotsDirectory;
        }
        private static string GetSnapshotFileName()
        {
            var now = DateTime.Now;
            var filename = $"snapshot_{now.Hour:D2}_{now.Minute:D2}_{now.Second:D2}.json";
            return filename;
        }
    }
}
