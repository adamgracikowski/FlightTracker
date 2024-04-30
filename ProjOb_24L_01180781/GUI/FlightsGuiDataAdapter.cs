namespace ProjOb_24L_01180781.GUI
{
    public class FlightsGuiDataAdapter : FlightsGUIData
    {
        public FlightsGuiDataAdapter(List<FlightDetails> flights)
        {
            _flights = flights;
        }
        public override int GetFlightsCount()
        {
            return _flights.Count;
        }
        public override ulong GetID(int index)
        {
            return _flights[index].ID;
        }
        public override WorldPosition GetPosition(int index)
        {
            return _flights[index].WorldPosition;
        }
        public override double GetRotation(int index)
        {
            return _flights[index].MapCoordRotation;
        }

        private List<FlightDetails> _flights;
    }
}
