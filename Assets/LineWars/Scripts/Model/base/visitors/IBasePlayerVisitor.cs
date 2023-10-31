namespace LineWars.Model
{
    public interface IBasePlayerVisitor<out T>
    {
        public T Visit(BasePlayer basePlayer);
        public T Visit(BasePlayerProjection projection);
    }
}