namespace LineWars.Model
{
    public class MovedUnit
    {
        public object Unit { get; }
        public object DestinationNode { get; }

        public MovedUnit(object unit, object destinationNode)
        {
            Unit = unit;
            DestinationNode = destinationNode;
        }
    }
}