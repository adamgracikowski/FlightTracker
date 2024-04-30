using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.Database.SQL.Visitors;
using ProjOb_24L_01180781.DataSource.Ftr;
using ProjOb_24L_01180781.DataSource.Tcp;

namespace ProjOb_24L_01180781.AviationItems
{
    public class Flight
        : IAviationItem, IPositionable
    {
        public string FtrAcronym { get; } = FtrAcronyms.Flight;
        public string TcpAcronym { get; } = TcpAcronyms.Flight;

        public UInt64 Id { get; set; }
        public DateTime TakeOffDateTime;
        public DateTime LandingDateTime;
        public UInt64 OriginId;
        public UInt64 TargetId;
        public string? TakeOffTime;
        public string? LandingTime;
        public DateTime? StartingDateTime;
        public Position Position;
        public Position StartingPosition
            = new(Position.Unknown, Position.Unknown, Position.Unknown);
        public UInt64 PlaneId;
        public UInt64[] CrewIds { get; set; }
        public UInt64[] LoadIds { get; set; }
        public object Lock { get; private set; } = new();

        public Flight(UInt64 id, DateTime takeOffDateTime, DateTime landingDateTime,
            UInt64[] crewIds, UInt64[] loadIds,
            UInt64? originId = null, UInt64? targetId = null, UInt64? planeId = null,
            string? takeOffTime = null, string? landingTime = null,
            Position? position = null)
        {
            Id = id;
            TakeOffDateTime = takeOffDateTime;
            LandingDateTime = landingDateTime;
            CrewIds = crewIds;
            LoadIds = loadIds;
            OriginId = originId ?? 0;
            TargetId = targetId ?? 0;
            PlaneId = planeId ?? 0;
            TakeOffTime = takeOffTime;
            LandingTime = landingTime;
            Position = position ?? new();
        }
        public IAviationItem Copy()
        {
            UInt64[] copyCrewIds = new UInt64[CrewIds.Length];
            Array.Copy(CrewIds, copyCrewIds, CrewIds.Length);
            UInt64[] copyLoadIds = new UInt64[LoadIds.Length];
            Array.Copy(LoadIds, copyLoadIds, LoadIds.Length);

            return new Flight(Id, TakeOffDateTime, LandingDateTime, copyCrewIds, copyLoadIds,
                OriginId, TargetId, PlaneId, TakeOffTime, LandingTime, Position.Copy());
        }
        public void UpdatePosition(double? longitude = null, double? latitude = null, double? amsl = null)
        {
            Position.Update(longitude, latitude, amsl);
            StartingPosition.Update(longitude, latitude, amsl);
            StartingDateTime = DateTime.UtcNow;
        }
        public Position GetPosition()
        {
            return Position.Copy();
        }
        public void AcceptQueryVisitor(QueryVisitor visitor)
        {
            visitor.RunQuery(this);
        }
    }
}
