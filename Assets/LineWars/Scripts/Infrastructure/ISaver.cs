namespace LineWars.Model
{
    public interface ISaver<in TValue>
    {
        public void Save(TValue value, int id);
    }
}
