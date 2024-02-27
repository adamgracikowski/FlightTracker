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
}
