namespace ProjOb_24L_01180781.AviationItems.Interfaces
{
    public interface IPositionable
    {
        Position GetPosition();
        void UpdatePosition(double? longitude = null, double? latitude = null, double? amsl = null);
    }
}
