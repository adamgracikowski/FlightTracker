namespace ProjOb_24L_01180781.Exceptions
{
    /// <summary>
    /// The base class of all the exceptions specific to the project.
    /// </summary>
    public abstract class AviationException
        : ApplicationException
    {
        public AviationException()
            : base() { }
        public AviationException(string? message)
            : base(message) { }
        public AviationException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}
