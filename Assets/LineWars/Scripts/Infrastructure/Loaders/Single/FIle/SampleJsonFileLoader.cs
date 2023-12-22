namespace LineWars.Model
{
    public class SampleJsonFileLoader<T>: FileDownloader<T>
    {
        public SampleJsonFileLoader() : base(
            new SimpleFilePathGenerator<T>("json"),
            new FromJsonConverter<T>())
        {
        }
    }
}