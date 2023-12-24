namespace LineWars.Model
{
    public class JsonFileSaver<TValue>: FileSaver<TValue>
    {
        public JsonFileSaver() : base(
            new SimpleFilePathGenerator<TValue>("json"),
            new ToJsonConverter<TValue>())
        {
        }
    }
}