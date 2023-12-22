namespace LineWars.Model
{ 
    public interface IConverter<in TIn, out TOut>
    {
        public TOut Convert(TIn value);
    }
}
