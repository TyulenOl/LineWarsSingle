using LineWars.Extensions.Attributes;
using UnityEngine;

namespace LineWars
{
    public abstract class Owned: MonoBehaviour
    {
        [SerializeField] [ReadOnlyInspector] protected Player owner;
        public Player Owner => owner;

        public virtual void SetOwner(Player newOwner)
        {
            owner = newOwner;
        }

        public static void Connect(Player player, Owned owned)
        {
            player.AddOwned(owned);
            owned.SetOwner(player);
        }
    }
}