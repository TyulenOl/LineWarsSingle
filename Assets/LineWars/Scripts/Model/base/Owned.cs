using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    /// <summary>
    /// класс, который объединяет все принадлежащее (ноды, юнитов и т.д.)
    /// </summary>
    public abstract class Owned: MonoBehaviour, IOwned<Owned, BasePlayer>
    {
        [Header("Accessory settings")]
        [SerializeField] [ReadOnlyInspector] protected BasePlayer basePlayer;
        public event Action<BasePlayer, BasePlayer> OwnerChanged;
        public event Action Replenished;

        public BasePlayer Owner
        {
            get => basePlayer;
            set => SetOwner(value);
        }
        
        private void SetOwner([MaybeNull]BasePlayer newBasePlayer)
        {
            var temp = basePlayer;
            basePlayer = newBasePlayer;
            OnSetOwner(temp, newBasePlayer);
            OwnerChanged?.Invoke(temp, newBasePlayer);
        }

        public void ConnectTo(BasePlayer basePlayer) => Connect(basePlayer, this);
        public static void Connect(BasePlayer basePlayer, Owned owned)
        {
            var otherOwner = owned.Owner;
            if (otherOwner != null)
            {
                owned.Owner = null;
                if (otherOwner != basePlayer)
                    otherOwner.RemoveOwned(owned);
            }
        
            
            basePlayer.AddOwned(owned);
            owned.Owner = basePlayer;
        }
        
        protected virtual void OnDisable()
        {
        }

        protected virtual void OnSetOwner(BasePlayer oldPlayer, BasePlayer newPlayer) {}

        public void Replenish()
        {
            OnReplenish();
            Replenished?.Invoke();
        }

        protected virtual void OnReplenish() {}
    }
}