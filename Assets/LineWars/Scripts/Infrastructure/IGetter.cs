
namespace LineWars.Model
{
    public interface IGetter<out T>
    {
        public bool CanGet();
        public T Get();
    }
}
