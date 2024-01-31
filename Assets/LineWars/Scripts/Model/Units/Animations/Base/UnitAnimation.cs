using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    public abstract class UnitAnimation : MonoBehaviour
    {
        [SerializeField] protected Unit ownerUnit;
        [ReadOnlyInspector] private bool isPlaying;
        public bool IsPlaying => isPlaying;
        [field: SerializeField] public UnityEvent<UnitAnimation> Started { get; private set; }
        [field: SerializeField] public UnityEvent<UnitAnimation> Ended { get; private set; }

        public abstract void Execute(AnimationContext context);

        protected void StartAnimation()
        {
            isPlaying = true;
            Started?.Invoke(this);
        }

        protected void EndAnimation()
        {
            isPlaying = false;
            Ended?.Invoke(this);   
        }
    }
}
