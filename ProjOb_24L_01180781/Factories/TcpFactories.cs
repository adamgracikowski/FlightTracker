using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.Exceptions;
using ProjOb_24L_01180781.Tcp;
using ProjOb_24L_01180781.Tools;

namespace ProjOb_24L_01180781.Factories
{
    public interface ITcpAviationFactory : IAviationFactory
    {
        IAviationItem Create(byte[] requested);
    }
    public class CrewTcpFactory : ITcpAviationFactory
    {
        public IAviationItem Create(byte[] bytes)
        {
            int offset = TcpMessageConstant.ExtendedAcronymLength;
            var bi = new ByteInterpreter(isLittleEndian: true);

            var fml = bi.GetUInt32(bytes, ref offset);

            if (bytes.Length - offset != fml)
            {
                var message = "invalid number of bytes for creating new Crew entity";
                throw new TcpFormatException(message);
            }

            var id = bi.GetUInt64(bytes, ref offset);
            var nameLength = bi.GetUInt16(bytes, ref offset);
            var name = bi.GetString(bytes, ref offset, nameLength);
            var age = bi.GetUInt16(bytes, ref offset);
            var phone = bi.GetString(bytes, ref offset,
                TcpMessageConstant.PersonPhoneNumberLength);
            var emailLength = bi.GetUInt16(bytes, ref offset);
            var email = bi.GetString(bytes, ref offset, emailLength);
            var practice = bi.GetUInt16(bytes, ref offset);
            var roleLetter = bi.GetString(bytes, ref offset, 1);
            
            if(!LetterToRole.TryGetValue(roleLetter, out var role))
            {
                var message = "unknown role for a Crew entity";
                throw new TcpFormatException(message);
            }

            return new Crew(id, name, age, phone, email, practice, role);

        }
        private static readonly Dictionary<string, string> LetterToRole = new()
        {
            { "A", "Attendant" },
            { "C", "Captain" },
            { "O", "Other" }
        };
    }
    public class PassengerTcpFactory : ITcpAviationFactory
    {
        public IAviationItem Create(byte[] bytes)
        {
            int offset = TcpMessageConstant.ExtendedAcronymLength;
            var bi = new ByteInterpreter(isLittleEndian: true);

            var fml = bi.GetUInt32(bytes, ref offset);

            if (bytes.Length - offset != fml)
            {
                var message = "invalid number of bytes for creating new Passenger entity";
                throw new TcpFormatException(message);
            }

            var id = bi.GetUInt64(bytes, ref offset);
            var nameLength = bi.GetUInt16(bytes, ref offset);
            var name = bi.GetString(bytes, ref offset, nameLength);
            var age = bi.GetUInt16(bytes, ref offset);
            var phone = bi.GetString(bytes, ref offset,
                TcpMessageConstant.PersonPhoneNumberLength);
            var emailLength = bi.GetUInt16(bytes, ref offset);
            var email = bi.GetString(bytes, ref offset, emailLength);
            var planeClassLetter = bi.GetString(bytes, ref offset, 1);
            
            if (!LetterToPlaneClass.TryGetValue(planeClassLetter, out var planeClass))
            {
                var message = "unknown plane class for a Passenger entity";
                throw new TcpFormatException(message);
            }

            var miles = bi.GetUInt64(bytes, ref offset);

            return new Passenger(id, name, age, phone, email, planeClass, miles);
        }
        private static readonly Dictionary<string, string> LetterToPlaneClass = new()
        {
            { "F", "First" },
            { "B", "Business" },
            { "E", "Economy" }
        };
    }
    public class CargoTcpFactory : ITcpAviationFactory
    {
        public IAviationItem Create(byte[] bytes)
        {
            int offset = TcpMessageConstant.ExtendedAcronymLength;
            var bi = new ByteInterpreter(isLittleEndian: true);

            var fml = bi.GetUInt32(bytes, ref offset);

            if (bytes.Length - offset != fml)
            {
                var message = "invalid number of bytes for creating new Cargo entity";
                throw new TcpFormatException(message);
            }

            var id = bi.GetUInt64(bytes, ref offset);
            var weight = bi.GetSingle(bytes, ref offset);
            var code = bi.GetString(bytes, ref offset,
                TcpMessageConstant.CargoCodeLength);
            var descriptionLength = bi.GetUInt16(bytes, ref offset);
            var description = bi.GetString(bytes, ref offset, descriptionLength);

            return new Cargo(id, weight, code, description);
        }
    }
    public class CargoPlaneTcpFactory : ITcpAviationFactory
    {
        public IAviationItem Create(byte[] bytes)
        {
            int offset = TcpMessageConstant.ExtendedAcronymLength;
            var bi = new ByteInterpreter(isLittleEndian: true);
            var fml = bi.GetUInt32(bytes, ref offset);

            if (bytes.Length - offset != fml)
            {
                var message = "invalid number of bytes for creating new CargoPlane entity";
                throw new TcpFormatException(message);
            }

            var id = bi.GetUInt64(bytes, ref offset);
            var serial = bi.GetString(bytes, ref offset,
                TcpMessageConstant.PlaneSerialLength);
            offset += TcpMessageConstant.PlaneMessageHole;
            var country = bi.GetString(bytes, ref offset,
                TcpMessageConstant.IsoCountryCodeLength);
            var modelLength = bi.GetUInt16(bytes, ref offset);
            var model = bi.GetString(bytes, ref offset, modelLength);
            var maxLoad = bi.GetSingle(bytes, ref offset);

            return new CargoPlane(id, serial, country, model, maxLoad);
        }
    }
    public class PassengerPlaneTcpFactory : ITcpAviationFactory
    {
        public IAviationItem Create(byte[] bytes)
        {
            int offset = TcpMessageConstant.ExtendedAcronymLength;
            var bi = new ByteInterpreter(isLittleEndian: true);

            var fml = bi.GetUInt32(bytes, ref offset);

            if (bytes.Length - offset != fml)
            {
                var message = "invalid number of bytes for creating new PassengerPlane entity";
                throw new TcpFormatException(message);
            }

            var id = bi.GetUInt64(bytes, ref offset);
            var serial = bi.GetString(bytes, ref offset,
                TcpMessageConstant.PlaneSerialLength);
            offset += TcpMessageConstant.PlaneMessageHole;
            var country = bi.GetString(bytes, ref offset,
                TcpMessageConstant.IsoCountryCodeLength);
            var modelLength = bi.GetUInt16(bytes, ref offset);
            var model = bi.GetString(bytes, ref offset, modelLength);
            var first = bi.GetUInt16(bytes, ref offset);
            var business = bi.GetUInt16(bytes, ref offset);
            var economy = bi.GetUInt16(bytes, ref offset);

            return new PassengerPlane(id, serial, country, model,
                                      new ClassSize(first, business, economy));
        }
    }
    public class AirportTcpFactory : ITcpAviationFactory
    {
        public IAviationItem Create(byte[] bytes)
        {
            int offset = TcpMessageConstant.ExtendedAcronymLength;
            var bi = new ByteInterpreter(isLittleEndian: true);

            var fml = bi.GetUInt32(bytes, ref offset);

            if (bytes.Length - offset != fml)
            {
                var message = "invalid number of bytes for creating new Airport entity";
                throw new TcpFormatException(message);
            }

            var id = bi.GetUInt64(bytes, ref offset);
            var nameLength = bi.GetUInt16(bytes, ref offset);
            var name = bi.GetString(bytes, ref offset, nameLength);
            var code = bi.GetString(bytes, ref offset,
                TcpMessageConstant.AirportCodeLenght);
            var longitude = bi.GetSingle(bytes, ref offset);
            var latitude = bi.GetSingle(bytes, ref offset);
            var amsl = bi.GetSingle(bytes, ref offset);
            var country = bi.GetString(bytes, ref offset,
                TcpMessageConstant.IsoCountryCodeLength);

            return new Airport(id, name, code, new Location(longitude, latitude, amsl), country);
        }
    }
    public class FlightTcpFactory : ITcpAviationFactory
    {
        public IAviationItem Create(byte[] bytes)
        {
            int offset = TcpMessageConstant.ExtendedAcronymLength;
            var bi = new ByteInterpreter(isLittleEndian: true);

            var fml = bi.GetUInt32(bytes, ref offset);

            if (bytes.Length - offset != fml)
            {
                var message = "invalid number of bytes for creating new Flight entity";
                throw new TcpFormatException(message);
            }

            var id = bi.GetUInt64(bytes, ref offset);
            var originId = bi.GetUInt64(bytes, ref offset);
            var targetId = bi.GetUInt64(bytes, ref offset);
            var takeOffTime = MsConverter.SinceEpochUtc(bi.GetInt64(bytes, ref offset));
            var landingTime = MsConverter.SinceEpochUtc(bi.GetInt64(bytes, ref offset));
            var planeId = bi.GetUInt64(bytes, ref offset);
            var crewIdsCount = bi.GetUInt16(bytes, ref offset);
            var crewIds = bi.GetUInt64(bytes, ref offset, crewIdsCount);
            var loadIdsCount = bi.GetUInt16(bytes, ref offset);
            var loadIds = bi.GetUInt64(bytes, ref offset, loadIdsCount);
            var location = new Location(latitude: Location.Unknown,
                                        longitude: Location.Unknown,
                                        amsl: Location.Unknown);

            return new Flight(id, originId, targetId, takeOffTime, landingTime,
                              location, planeId, crewIds, loadIds);
        }
    }
}
