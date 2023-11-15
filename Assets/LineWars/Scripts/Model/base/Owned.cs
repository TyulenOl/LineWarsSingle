using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    /// <summary>
    /// класс, который объединяет все принадлежащее (ноды, юнитов и т.д.)
    /// </summary>
    public abstract class Owned : MonoBehaviour, IOwned
    {
        [Header("Accessory settings")] [SerializeField, ReadOnlyInspector]
        protected BasePlayer basePlayer;

        public event Action<BasePlayer, BasePlayer> OwnerChanged;
        public event Action Replenished;

        public int OwnerId => Owner != null ? Owner.Id : -1;

        public BasePlayer Owner
        {
            get => basePlayer;
            set => SetOwner(value);
        }

        private void SetOwner([MaybeNull] BasePlayer newBasePlayer)
        {
            var temp = basePlayer;
            basePlayer = newBasePlayer;
            OnSetOwner(temp, newBasePlayer);
            OwnerChanged?.Invoke(temp, newBasePlayer);
        }

        public void ConnectTo(int basePlayerID)
        {
            var player = basePlayerID != -1 ? SingleGame.Instance.AllPlayers[basePlayerID] : null;
            Connect(player, this);
        }

        public void Replenish()
        {
            OnReplenish();
            Replenished?.Invoke();
        }

        protected virtual void OnReplenish()
        {
        }

        protected virtual void OnSetOwner(BasePlayer oldPlayer, BasePlayer newPlayer)
        {
        }

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
    }
}