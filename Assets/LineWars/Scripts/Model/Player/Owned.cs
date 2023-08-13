using LineWars.Extensions.Attributes;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public abstract class Owned: MonoBehaviour
    {
        [SerializeField] [ReadOnlyInspector] protected IOwner owner;
        public IOwner Owner => owner;

        public virtual void SetOwner(IOwner newOwner)
        {
            owner = newOwner;
        }

        public static void Connect(IOwner owner, Owned owned)
        {
            owner.AddOwned(owned);
            owned.SetOwner(owner);
        }
    }
}