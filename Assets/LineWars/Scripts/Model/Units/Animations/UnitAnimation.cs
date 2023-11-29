using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    public abstract class UnitAnimation : MonoBehaviour
    {
        [SerializeField] protected Unit ownerUnit;
        [ReadOnlyInspector] private bool isPlaying;
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

        public abstract void Execute(AnimationContext context);
    }
}
