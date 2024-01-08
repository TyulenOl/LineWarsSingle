namespace LineWars.Model
{
    /// <summary>
    /// Декорирует произвольный ISaver на доп преобразование перед сохранением (SRP)
    /// </summary>
    public class SaverConvertDecorator<TIn, TOut>: ISaver<TIn>
    {
        private readonly ISaver<TOut> innerSaver;
        private readonly IConverter<TIn, TOut> converter;

        public SaverConvertDecorator(ISaver<TOut> innerSaver, IConverter<TIn, TOut> converter)
        {
            this.innerSaver = innerSaver;
            this.converter = converter;
        }

        public void Save(TIn value, int id)
        {
            var convert = converter.Convert(value);
            innerSaver.Save(convert, id);
        }
    }
}