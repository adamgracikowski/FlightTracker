namespace ProjOb_24L_01180781.Tools
{
    public class SnapshotDetails
    {
        public string Name { get; private set; }
        public int CollectionCount { get; private set; }
        public TimeSpan TimeTaken { get; private set; }
        public SnapshotDetails(string name, int collectionCount, TimeSpan timeTaken)
        {
            Name = name;
            CollectionCount = collectionCount;
            TimeTaken = timeTaken;
        }
    }
}
