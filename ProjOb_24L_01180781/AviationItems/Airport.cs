using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.Database.SQL.Visitors;
using ProjOb_24L_01180781.DataSource.Ftr;
using ProjOb_24L_01180781.DataSource.Tcp;
using ProjOb_24L_01180781.Media;

namespace ProjOb_24L_01180781.AviationItems
{
    public class Airport
        : IAviationItem, IReportable, IPositionable
    {
        public string FtrAcronym { get; } = FtrAcronyms.Airport;
        public string TcpAcronym { get; } = TcpAcronyms.Airport;

        public UInt64 Id { get; set; }
        public string? Name;
        public string? Code;
        public Position Position;
        public string? Country;
        public object Lock { get; private set; } = new();

        public Airport(UInt64 id, string? name = null, string? code = null, Position? position = null, string? country = null)
        {
            Id = id;
            Name = name;
            Code = code;
            Position = position ?? new Position();
            Country = country;
        }
        public string AcceptMediaReport(IMedia media)
        {
            return media.MakeReport(this);
        }
        public IAviationItem Copy()
        {
            return new Airport(Id, Name, Code, Position?.Copy(), Country);
        }
        public Position GetPosition()
        {
            return Position.Copy();
        }
        public void UpdatePosition(double? longitude = null, double? latitude = null, double? amsl = null)
        {
            Position.Update(longitude, latitude, amsl);
        }
        public void AcceptQueryVisitor(QueryVisitor visitor)
        {
            visitor.RunQuery(this);
        }
    }
}