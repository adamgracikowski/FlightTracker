namespace ProjOb_24L_01180781.AviationItems.Interfaces
{
    public interface IHasId
    {
        UInt64 Id { get; set; }
        void UpdateId(UInt64 id)
        {
            Id = id;
        }
    }
}
