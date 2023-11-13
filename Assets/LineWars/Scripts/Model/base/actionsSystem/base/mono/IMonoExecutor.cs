namespace LineWars.Model
{
    public interface IMonoExecutor:
        IExecutor,
        IMonoBehaviorImplementation
    {
        public T Accept<T>(IMonoExecutorVisitor<T> visitor);
    }
}