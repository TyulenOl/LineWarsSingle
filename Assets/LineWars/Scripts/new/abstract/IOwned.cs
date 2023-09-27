namespace LineWars.Model
{
    public interface IOwned: IReadOnlyOwned
    {
        public new IBasePlayer Owner { get; }
        public void SetOwner(IBasePlayer basePlayer);

        public void ConnectTo(IBasePlayer basePlayer)
        {
            SetOwner(basePlayer);
            basePlayer.AddOwned(this);
        }
    }
}