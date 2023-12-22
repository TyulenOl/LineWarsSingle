namespace LineWars.Model
{
    public class SampleJsonFileSaver<TValue>: FileSaver<TValue>
    {
        public SampleJsonFileSaver() : base(
            new SimpleFilePathGenerator<TValue>("json"),
            new ToJsonConverter<TValue>())
        {
        }
    }
}