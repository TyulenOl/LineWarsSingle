namespace LineWars.Model
{
    public interface IRoadUpper
    {
        public bool CanUpRoad(Edge edge);
        public void UpRoad(Edge edge);
    }
}