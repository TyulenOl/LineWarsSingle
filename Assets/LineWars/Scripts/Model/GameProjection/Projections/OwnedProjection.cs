namespace LineWars.Model
{
    public abstract class OwnedProjection
        : IOwned<OwnedProjection, BasePlayerProjection>
    {
        public BasePlayerProjection Owner { get; set; }

        public void ConnectTo(BasePlayerProjection basePlayer)
        {
            
            var otherOwner = Owner;
            if (otherOwner != null)
            {
                Owner = null;
                if (otherOwner != basePlayer)
                    otherOwner.RemoveOwned(this);
               }

            basePlayer.AddOwned(this);
            Owner = basePlayer;     
        }

        public virtual void Replenish() { }
    }
}
