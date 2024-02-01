namespace LineWars.Model
{
    public interface IStackableEffect
    {
        public void Stack(IStackableEffect effect);

        public bool CanStack(IStackableEffect effect);
    }
}
