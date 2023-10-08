namespace LineWars.Model
{
    public interface IExecutor : IReadOnlyExecutor
    {
        public new int CurrentActionPoints { get; set; }
    }
}