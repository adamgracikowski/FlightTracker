namespace ProjOb_24L_01180781.AviationItems
{
    public class ClassSize
    {
        public static readonly UInt16 Unknown = 0;
        public UInt16 First;
        public UInt16 Business;
        public UInt16 Economy;
        public ClassSize(UInt16? first, UInt16? business, UInt16? economy)
        {
            First = first ?? Unknown;
            Business = business ?? Unknown;
            Economy = economy ?? Unknown;
        }
        public ClassSize()
            : this(Unknown, Unknown, Unknown)
        { }

        public ClassSize Copy()
        {
            return new ClassSize(First, Business, Economy);
        }
    }
}
