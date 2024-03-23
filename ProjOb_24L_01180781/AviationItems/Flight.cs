using ProjOb_24L_01180781.Ftr;
using ProjOb_24L_01180781.Tcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public class Flight
        : IAviationItem
    {
        public string FtrAcronym { get; } = FtrAcronyms.Flight;
        public string TcpAcronym { get; } = TcpAcronyms.Flight;

        public UInt64 Id { get; private set; }
        public UInt64 OriginId { get; private set; }
        public UInt64 TargetId { get; private set; }
        public string TakeOffTime { get; private set; }
        public string LandingTime { get; private set; }
        public DateTime? TakeOffDateTime { get; private set; }
        public DateTime? LandingDateTime { get; private set; }
        public Location Location { get; set; }
        public UInt64 PlaneId { get; private set; }
        public UInt64[] CrewIds { get; private set; }
        public UInt64[] LoadIds { get; private set; }

        public Flight(UInt64 id, UInt64 originId, UInt64 targetId, string takeOffTime, string landingTime,
            Location location, UInt64 planeId, UInt64[] crewIds, UInt64[] loadIds,
            DateTime? takeOffDateTiem = null, DateTime? landingDateTime = null)
        {
            Id = id;
            OriginId = originId;
            TargetId = targetId;
            TakeOffTime = takeOffTime;
            LandingTime = landingTime;
            Location = location;
            PlaneId = planeId;
            CrewIds = crewIds;
            LoadIds = loadIds;
            TakeOffDateTime = takeOffDateTiem;
            LandingDateTime = landingDateTime;
        }
    }
}
