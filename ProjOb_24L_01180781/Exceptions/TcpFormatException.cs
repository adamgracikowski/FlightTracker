namespace ProjOb_24L_01180781.Exceptions
{
    /// <summary>
    /// Represents all the exceptions connected with TCP messages formatting issues.
    /// </summary>
    public class TcpFormatException : AviationException
    {
        public TcpFormatException()
            : base() { }
        public TcpFormatException(string? message)
            : base(message) { }
        public TcpFormatException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}
