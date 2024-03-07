using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Exceptions
{
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
