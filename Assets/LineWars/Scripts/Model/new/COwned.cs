using System;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public abstract class COwned
    {
        protected CBasePlayer basePlayer;
        public event Action<CBasePlayer, CBasePlayer> OwnerChanged;
        public event Action Replenished;
        public CBasePlayer Owner => basePlayer;

        public void SetOwner([MaybeNull]CBasePlayer newBasePlayer)
        {
            var temp = basePlayer;
            basePlayer = newBasePlayer;
            OnSetOwner(temp, newBasePlayer);
            OwnerChanged?.Invoke(temp, newBasePlayer);
        }

        public static void Connect(CBasePlayer basePlayer, COwned owned)
        {
            basePlayer.AddOwned(owned);
            owned.SetOwner(basePlayer);
        }
        
        protected virtual void OnDisable()
        {
        }

        protected virtual void OnSetOwner(CBasePlayer oldPlayer, CBasePlayer newPlayer) {}

        public void Replenish()
        {
            OnReplenish();
            Replenished?.Invoke();
        }

        protected virtual void OnReplenish() {}
    }
}