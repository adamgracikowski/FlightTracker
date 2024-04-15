using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.DataSource;
using ProjOb_24L_01180781.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.DataSource.Ftr
{
    public interface IFtrAviationFactory : IAviationFactory
    {
        IAviationItem Create(string[] requested);
    }
    public class CrewFtrFactory
    : IFtrAviationFactory
    {
        public IAviationItem Create(string[] itemDetails)
        {
            return new Crew(
                id: ulong.Parse(itemDetails[1]),
                name: itemDetails[2],
                age: ulong.Parse(itemDetails[3]),
                phone: itemDetails[4],
                email: itemDetails[5],
                practice: ushort.Parse(itemDetails[6]),
                role: itemDetails[7]
            );
        }
    }
    public class PassengerFtrFactory
    : IFtrAviationFactory
    {
        public IAviationItem Create(string[] itemDetails)
        {
            return new Passenger(
                id: ulong.Parse(itemDetails[1]),
                name: itemDetails[2],
                age: ulong.Parse(itemDetails[3]),
                phone: itemDetails[4],
                email: itemDetails[5],
                planeClass: itemDetails[6],
                miles: ulong.Parse(itemDetails[7])
            );
        }
    }
    public class CargoFtrFactory
    : IFtrAviationFactory
    {
        public IAviationItem Create(string[] itemDetails)
        {
            return new Cargo(
                id: ulong.Parse(itemDetails[1]),
                weight: float.Parse(itemDetails[2]),
                code: itemDetails[3],
                description: itemDetails[4]
            );
        }
    }
    public class CargoPlaneFtrFactory
    : IFtrAviationFactory
    {
        public IAviationItem Create(string[] itemDetails)
        {
            return new CargoPlane(
                id: ulong.Parse(itemDetails[1]),
                serial: itemDetails[2],
                country: itemDetails[3],
                model: itemDetails[4],
                maxLoad: float.Parse(itemDetails[5])
            );
        }
    }
    public class PassengerPlaneFtrFactory
    : IFtrAviationFactory
    {
        public IAviationItem Create(string[] itemDetails)
        {
            return new PassengerPlane(
                id: ulong.Parse(itemDetails[1]),
                serial: itemDetails[2],
                country: itemDetails[3],
                model: itemDetails[4],
                classSize: new ClassSize(
                    first: ushort.Parse(itemDetails[5]),
                    business: ushort.Parse(itemDetails[6]),
                    economy: ushort.Parse(itemDetails[7]))
            );
        }
    }
    public class AirportFtrFactory
    : IFtrAviationFactory
    {
        public IAviationItem Create(string[] itemDetails)
        {
            return new Airport(
                id: ulong.Parse(itemDetails[1]),
                name: itemDetails[2],
                code: itemDetails[3],
                location: new Position(
                    longitude: float.Parse(itemDetails[4]),
                    latitude: float.Parse(itemDetails[5]),
                    amsl: float.Parse(itemDetails[6])),
                country: itemDetails[7]
            );
        }
    }
    public class FlightFtrFactory
    : IFtrAviationFactory
    {
        public IAviationItem Create(string[] itemDetails)
        {
            var separator = new char[] { '[', ';', ']' };
            var takeOffTime = itemDetails[4];
            var takeOffDateTime = TimeConverter.ParseExact(takeOffTime);
            var landingTime = itemDetails[5];
            var landingDateTime = TimeConverter.ParseExact(landingTime);
            if (takeOffDateTime > landingDateTime)
            {
                landingDateTime = landingDateTime.AddDays(1);
                landingTime = TimeConverter.FromDateTimeToFormatString(landingDateTime);
            }

            return new Flight(
                id: ulong.Parse(itemDetails[1]),
                originId: ulong.Parse(itemDetails[2]),
                targetId: ulong.Parse(itemDetails[3]),
                takeOffTime: takeOffTime,
                landingTime: landingTime,
                location: new Position(
                    longitude: float.Parse(itemDetails[6]),
                    latitude: float.Parse(itemDetails[7]),
                    amsl: float.Parse(itemDetails[8])),
                planeId: ulong.Parse(itemDetails[9]),
                crewIds: itemDetails[10].ParseToArraySeparated<ulong>(separator),
                loadIds: itemDetails[11].ParseToArraySeparated<ulong>(separator),
                takeOffDateTime: takeOffDateTime,
                landingDateTime: landingDateTime
            );
        }
    }
}
