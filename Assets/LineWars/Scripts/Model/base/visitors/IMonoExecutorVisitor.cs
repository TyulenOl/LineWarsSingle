namespace LineWars.Model
{
    public interface IMonoExecutorVisitor<out T>
    {
        public T Visit(Unit unit);
    }
}