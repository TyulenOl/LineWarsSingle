namespace LineWars.Model
{
    public class JsonFileLoader<T>: FileDownloader<T>
    {
        public JsonFileLoader() : base(
            new SimpleFilePathGenerator<T>("json"),
            new FromJsonConverter<T>())
        {
        }
    }
}