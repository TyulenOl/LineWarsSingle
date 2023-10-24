namespace LineWars.Model
{
    public abstract class AIAction
    {
        public IReadOnlyGameProjection InitialProjection { get; protected set; }
        public IReadOnlyGameProjection NewProjection { get; protected set; }
    }
}
