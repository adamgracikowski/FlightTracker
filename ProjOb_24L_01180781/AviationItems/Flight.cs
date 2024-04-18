using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.DataSource.Ftr;
using ProjOb_24L_01180781.DataSource.Tcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public class Flight
        : IAviationItem, IPositionable
    {
        public string FtrAcronym { get; } = FtrAcronyms.Flight;
        public string TcpAcronym { get; } = TcpAcronyms.Flight;

        public UInt64 Id { get; set; }
        public UInt64 OriginId { get; set; }
        public UInt64 TargetId { get; set; }
        public string TakeOffTime { get; private set; }
        public string LandingTime { get; private set; }
        public DateTime TakeOffDateTime { get; private set; }
        public DateTime LandingDateTime { get; private set; }
        public DateTime StartingDateTime { get; set; }
        public Position Position { get; set; }
        public Position StartingPosition { get; set; }
            = new(Position.Unknown, Position.Unknown, Position.Unknown);
        public UInt64 PlaneId { get; set; }
        public UInt64[] CrewIds { get; set; }
        public UInt64[] LoadIds { get; set; }
        public object Lock { get; private set; } = new();

        public Flight(UInt64 id, UInt64 originId, UInt64 targetId, string takeOffTime, string landingTime,
            Position position, UInt64 planeId, UInt64[] crewIds, UInt64[] loadIds,
            DateTime takeOffDateTime, DateTime landingDateTime)
        {
            Id = id;
            OriginId = originId;
            TargetId = targetId;
            TakeOffTime = takeOffTime;
            LandingTime = landingTime;
            Position = position;
            PlaneId = planeId;
            CrewIds = crewIds;
            LoadIds = loadIds;
            TakeOffDateTime = takeOffDateTime;
            LandingDateTime = landingDateTime;
        }
        public IAviationItem Copy()
        {
            UInt64[] copyCrewIds = new UInt64[CrewIds.Length];
            Array.Copy(CrewIds, copyCrewIds, CrewIds.Length);
            UInt64[] copyLoadIds = new UInt64[LoadIds.Length];
            Array.Copy(LoadIds, copyLoadIds, LoadIds.Length);
            return new Flight(Id, OriginId, TargetId,
                TakeOffTime, LandingTime, Position.Copy(),
                PlaneId, copyCrewIds, copyLoadIds,
                TakeOffDateTime, LandingDateTime);
        }
        public void UpdatePosition(Single longitude, Single latitude, Single? amsl = null)
        {
            Position.Update(longitude, latitude, amsl);
            StartingPosition.Update(longitude, latitude, amsl);
            StartingDateTime = DateTime.UtcNow;
        }
        public Position GetPosition()
        {
            return Position.Copy();
        }
    }
}
