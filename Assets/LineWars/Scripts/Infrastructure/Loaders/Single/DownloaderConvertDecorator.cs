using LineWars.Controllers;

namespace LineWars.Model
{
    /// <summary>
    /// Декорирует произвольный IDownloader на доп преобразование перед сохранением (SRP)
    /// </summary>
    public class DownloaderConvertDecorator<TIn, TOut>: IDownloader<TIn>
    {
        private readonly IDownloader<TOut> innerLoader;
        private readonly IConverter<TOut, TIn> converter;

        public DownloaderConvertDecorator(IDownloader<TOut> innerLoader, IConverter<TOut, TIn> converter)
        {
            this.innerLoader = innerLoader;
            this.converter = converter;
        }
        
        public TIn Load(int id)
        {
            var value = innerLoader.Load(id);
            return converter.Convert(value);
        }
    }
}