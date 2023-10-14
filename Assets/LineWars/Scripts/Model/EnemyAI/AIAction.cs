namespace LineWars.Model
{
    public abstract class AIAction
    {
        public IReadOnlyGameProjection InitialProjection { get; private set; }
        public IReadOnlyGameProjection NewProjection { get; private set; }
    }
}
