using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    /// <summary>
    /// Декорирует произвольный IAllDownloader на доп преобразование перед сохранением (SRP)
    /// </summary>
    public class AllDownloaderConvertDecorator<TIn, TOut> : IAllDownloader<TIn>
    {
        private readonly IAllDownloader<TOut> innerLoader;
        private readonly IConverter<TOut, TIn> converter;

        public IEnumerable<TIn> LoadAll()
        {
            return innerLoader.LoadAll()
                .Select(converter.Convert);
        }
    }
}