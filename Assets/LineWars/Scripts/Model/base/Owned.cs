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
        [SerializeField] [ReadOnlyInspector] protected BasePlayer basePlayer;
        public BasePlayer BasePlayer => basePlayer;

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

        protected virtual void OnSetOwner(BasePlayer oldPlayer, BasePlayer newPlayer) {}
    }
}