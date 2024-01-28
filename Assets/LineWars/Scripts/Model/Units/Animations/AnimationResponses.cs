using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace LineWars.Model
{
    public enum AnimationResponseType
    {
        MeleeDamaged,
        DistanceDamaged,
        SwingDamaged,
        ShotUpDamaged,
        ShotBottomDamaged,
        ComeTo,
        Throw,
        Rammed,
        MeleeDied,
        DistanceDied,
        RammedDied,
        ShotUpDied,
        ShotBottomDied,
        SwingDied,
        UpArmored,
        TargetPowerBasedAttacked,
        Healed,
        Sacrificed
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Unit))]
    public class AnimationResponses : MonoBehaviour
    {
        [SerializeField] private UnitAnimation defaultDeathAnimation;
        [SerializeField] private SerializedDictionary<AnimationResponseType, UnitAnimation> animations;

        private bool isDying;
        public UnitAnimation CurrentDeathAnimation { get; private set; }

        private void Start()
        {
            CurrentDeathAnimation = defaultDeathAnimation;
        }

        public UnitAnimation Respond(AnimationResponseType responseType, AnimationContext animationContext)
        {
            if (isDying)
                return null;
            if (!animations.ContainsKey(responseType))
                return null;
            animations[responseType].Execute(animationContext);
            return animations[responseType];
        }

        public bool CanRespond(AnimationResponseType responseType)
        {
            return animations.ContainsKey(responseType);
        }

        public bool TrySetDeathAnimation(AnimationResponseType responseType)
        {
            return TrySetDeathAnimation(responseType, out var _);
        }

        public bool TrySetDeathAnimation(AnimationResponseType responseType, out UnitAnimation deathAnimation)
        {
            if(!animations.ContainsKey(responseType)) 
            {
                deathAnimation = null;
                return false;
            }

            deathAnimation = animations[responseType];
            CurrentDeathAnimation = deathAnimation;
            return true;
        }

        public void NullifyDeathAnimation()
        { 
            CurrentDeathAnimation = null; 
        }

        public void SetDefaultDeathAnimation()
        {
            CurrentDeathAnimation = defaultDeathAnimation;
        }

        public void PlayDeathAnimation()
        {
            isDying = true;
            StopAllAnimations();
            var unit = GetComponent<Unit>();
            var context = new AnimationContext()
            {
                TargetUnit = unit,
                TargetNode = unit.Node
            };
            CurrentDeathAnimation.Execute(context);
        }

        private void StopAllAnimations()
        {
            foreach(var animation in animations.Values)
            {
                if(animation is UnitStoppingAnimation stoppingAnimation)
                {
                    stoppingAnimation.Stop();
                }
            }
        }
    }
}
