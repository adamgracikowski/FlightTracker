using ProjOb_24L_01180781.DataSource.Ftr;

namespace ProjOb_24L_01180781.Exceptions
{
    /// <summary>
    /// Represents all the exceptions connected with .ftr file formatting issues.
    /// </summary>
    public class FtrFormatException
        : AviationException
    {
        public FtrFileContext? Context { get; private set; }
        public FtrFormatException()
            : base() { }
        public FtrFormatException(string? message, FtrFileContext? context = null)
            : base(CreateMessage(message, context))
        {
            Context = context;
        }
        public FtrFormatException(string? message, Exception? innerException, FtrFileContext? context = null)
            : base(CreateMessage(message, context), innerException)
        {
            Context = context;
        }
        private static string? CreateMessage(string? message, FtrFileContext? context = null)
        {
            if (message is not null && context is not null)
            {
                message += $" in file {context.Filename} (in line {context.LineNumber})";
            }
            return message;
        }
    }
}
