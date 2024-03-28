using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.DataSource.Tcp
{
    /// <summary>
    /// Represents a set of constants specific for TCP messages.
    /// </summary>
    public static class TcpMessageConstant
    {
        public static readonly int ExtendedAcronymLength = 3;
        public static readonly int PersonPhoneNumberLength = 12;
        public static readonly int CargoCodeLength = 6;
        public static readonly int PlaneSerialLength = 6;
        public static readonly int IsoCountryCodeLength = 3;
        public static readonly int AirportCodeLenght = 3;
        public static readonly int PlaneMessageHole = 4;
    }
}
