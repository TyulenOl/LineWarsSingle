namespace LineWars.Model
{
    public interface IMonoExecutor:
        IExecutor
    {
        public T Accept<T>(IMonoExecutorVisitor<T> visitor);
    }
}