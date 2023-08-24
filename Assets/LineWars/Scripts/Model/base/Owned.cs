using System;
using LineWars.Extensions.Attributes;
using LineWars.Model;
using UnityEngine;
using UnityEngine.Serialization;

namespace LineWars.Model
{
    /// <summary>
    /// класс, который объединяет все принадлежащее (ноды, юнитов и т.д.)
    /// </summary>
    public abstract class Owned: MonoBehaviour
    {
        [Header("Accessory settings")]
        [SerializeField] [ReadOnlyInspector] protected BasePlayer basePlayer;
        public BasePlayer Owner => basePlayer;

        public void SetOwner(BasePlayer newBasePlayer)
        {
            var temp = basePlayer;
            basePlayer = newBasePlayer;
            OnSetOwner(temp, newBasePlayer);
        }

        public static void Connect(BasePlayer basePlayer, Owned owned)
        {
            basePlayer.AddOwned(owned);
            owned.SetOwner(basePlayer);
        }

        protected virtual void OnDisable()
        {
            if(Owner != null)
                Owner.RemoveOwned(this);
        }

        protected virtual void OnSetOwner(BasePlayer oldPlayer, BasePlayer newPlayer) {}
    }
}