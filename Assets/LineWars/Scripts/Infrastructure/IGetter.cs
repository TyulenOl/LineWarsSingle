
namespace LineWars.Model
{
    public interface IGetter<T>
    {
        public bool CanGet();

        public T Get();
    }
}
