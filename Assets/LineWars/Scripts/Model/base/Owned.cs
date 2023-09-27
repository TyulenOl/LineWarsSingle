using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Extensions.Attributes;
using LineWars.Model;
using UnityEngine;
using UnityEngine.Serialization;

namespace LineWars.Model
{
    /// <summary>
    /// класс, который объединяет все принадлежащее (ноды, юнитов и т.д.)
    /// </summary>
    public abstract class Owned: MonoBehaviour, IOwned
    {
        [Header("Accessory settings")]
        [SerializeField] [ReadOnlyInspector] protected BasePlayer basePlayer;
        public event Action<BasePlayer, BasePlayer> OwnerChanged;
        public event Action Replenished;
        public BasePlayer Owner => basePlayer;
        IBasePlayer IOwned.Owner => basePlayer;
        IReadOnlyBasePlayer IReadOnlyOwned.Owner => basePlayer;
        
        public void SetOwner([MaybeNull]BasePlayer newBasePlayer)
        {
            
            var temp = basePlayer;
            basePlayer = newBasePlayer;
            OnSetOwner(temp, newBasePlayer);
            OwnerChanged?.Invoke(temp, newBasePlayer);
        }
        void IOwned.SetOwner(IBasePlayer newBasePlayer) => SetOwner((BasePlayer)newBasePlayer);

        public static void Connect(BasePlayer basePlayer, Owned owned)
        {
            basePlayer.AddOwned(owned);
            owned.SetOwner(basePlayer);
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