using Mono.Cecil;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    public abstract class UnitAnimation : MonoBehaviour
    {
        protected Unit ownerUnit;
        private bool isPlaying;
        public bool IsPlaying 
        {
            get => isPlaying;
            protected set
            {
                var oldValue = isPlaying;
                isPlaying = value;
                if(oldValue != value)
                {
                    if(value)
                        Started.Invoke(this);
                    else
                        Ended.Invoke(this);
                }
            }
        }
        [field: SerializeField] public UnityEvent<UnitAnimation> Started { get; private set; }
        [field: SerializeField] public UnityEvent<UnitAnimation> Ended { get; private set; }

        protected virtual void Start()
        {
            var unit = GetComponent<Unit>();
            Initialize(unit);
        }
        public virtual void Initialize(Unit ownerUnit)
        {
            this.ownerUnit = ownerUnit;
        }

        public abstract void Execute(AnimationContext context);
    }
}
