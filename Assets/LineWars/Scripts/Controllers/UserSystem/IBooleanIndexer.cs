namespace LineWars.Controllers
{
    public interface IBooleanIndexer
    {
        public bool this[string key] { get; set; }
    }
}