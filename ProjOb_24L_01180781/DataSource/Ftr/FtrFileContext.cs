namespace ProjOb_24L_01180781.DataSource.Ftr
{
    public class FtrFileContext
    {
        public string Filename { get; set; }
        public ulong LineNumber { get; set; }

        public FtrFileContext(string filename, ulong lineNumber)
        {
            Filename = filename;
            LineNumber = lineNumber;
        }
    }
}
