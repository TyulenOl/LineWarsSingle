namespace LineWars.Model
{
    public class JsonFileAllDownloader<T>: FileAllDownLoader<T>
    {
        public JsonFileAllDownloader() 
            : base(new SimpleFilePathGenerator<T>("json"), 
                new FromJsonConverter<T>())
        {
        }
    }
}