namespace LineWars.Model
{
    public interface IIndexer<TValue>
    {
        public TValue this[int id]
        {
            get;
            set;
        }
    }
}