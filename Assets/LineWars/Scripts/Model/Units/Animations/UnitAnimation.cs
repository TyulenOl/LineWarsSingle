using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    public abstract class UnitAnimation : MonoBehaviour
    {
        protected Unit ownerUnit;
        [field: SerializeField] public UnityEvent<UnitAnimation> Started { get; private set; }
        [field: SerializeField] public UnityEvent<UnitAnimation> Ended { get; private set; }

        public virtual void Initialize(Unit ownerUnit)
        {
            this.ownerUnit = ownerUnit;
        }
    }
}
