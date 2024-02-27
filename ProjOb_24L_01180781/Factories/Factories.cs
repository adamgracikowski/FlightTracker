using ProjOb_24L_01180781.AviationItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Factories
{
    public class CrewFactory
        : IAviationFactory
    {
        public IAviationItem Create(string[] itemDetails)
        {
            return new Crew(
                id: UInt64.Parse(itemDetails[1]),
                name: itemDetails[2],
                age: UInt64.Parse(itemDetails[3]),
                phone: itemDetails[4],
                email: itemDetails[5],
                practice: UInt16.Parse(itemDetails[6]),
                role: itemDetails[7]
            );
        }
    }
    public class PassengerFactory
    : IAviationFactory
    {
        public IAviationItem Create(string[] itemDetails)
        {
            return new Passenger(
                id: UInt64.Parse(itemDetails[1]),
                name: itemDetails[2],
                age: UInt64.Parse(itemDetails[3]),
                phone: itemDetails[4],
                email: itemDetails[5],
                planeClass: itemDetails[6],
                miles: UInt64.Parse(itemDetails[7])
            );
        }
    }
    public class CargoFactory
    : IAviationFactory
    {
        public IAviationItem Create(string[] itemDetails)
        {
            return new Cargo(
                id: UInt64.Parse(itemDetails[1]),
                weight: Single.Parse(itemDetails[2]),
                code: itemDetails[3],
                description: itemDetails[4]
            );
        }
    }
    public class CargoPlaneFactory
    : IAviationFactory
    {
        public IAviationItem Create(string[] itemDetails)
        {
            return new CargoPlane(
                id: UInt64.Parse(itemDetails[1]),
                serial: itemDetails[2],
                country: itemDetails[3],
                model: itemDetails[4],
                maxLoad: Single.Parse(itemDetails[5])
            );
        }
    }
    public class PassengerPlaneFactory
    : IAviationFactory
    {
        public IAviationItem Create(string[] itemDetails)
        {
            return new PassengerPlane(
                id: UInt64.Parse(itemDetails[1]),
                serial: itemDetails[2],
                country: itemDetails[3],
                model: itemDetails[4],
                classSize: new ClassSize(
                    first: UInt16.Parse(itemDetails[5]),
                    business: UInt16.Parse(itemDetails[6]),
                    economy: UInt16.Parse(itemDetails[7]))
            );
        }
    }
    public class AirportFactory
    : IAviationFactory
    {
        public IAviationItem Create(string[] itemDetails)
        {
            return new Airport(
                id: UInt64.Parse(itemDetails[1]),
                name: itemDetails[2],
                code: itemDetails[3],
                location: new Location(
                    longitude: Single.Parse(itemDetails[4]),
                    latitude: Single.Parse(itemDetails[5]),
                    amsl: Single.Parse(itemDetails[6])),
                country: itemDetails[7]
            );
        }
    }
}
