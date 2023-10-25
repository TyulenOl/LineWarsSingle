namespace LineWars.Model
{
    public interface IExecutorVisitor<out T>
    {
        public T Visit(Unit unit);
        public T Visit(UnitProjection unitProjection);
    }
}