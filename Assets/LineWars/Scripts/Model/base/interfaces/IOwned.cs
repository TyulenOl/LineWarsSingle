namespace LineWars.Model
{
    public interface IOwned<TOwned, TPlayer>
        
        #region Сonstraints
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TOwned, TPlayer>
        #endregion
    {
        public TPlayer Owner { get; set; }
        public void Replenish();
        public void ConnectTo(TPlayer basePlayer);
    }
}