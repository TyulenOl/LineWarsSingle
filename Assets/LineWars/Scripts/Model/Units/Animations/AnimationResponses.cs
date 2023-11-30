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
        SwingDied
    }

    [DisallowMultipleComponent]
    public class AnimationResponses : MonoBehaviour
    {
        [SerializeField] private UnitAnimation defaultDeathAnimation;
        [SerializeField] private SerializedDictionary<AnimationResponseType, UnitAnimation> animations;

        public UnitAnimation CurrentDeathAnimation { get; private set; }

        private void Start()
        {
            CurrentDeathAnimation = defaultDeathAnimation;
        }

        public UnitAnimation Respond(AnimationResponseType responseType, AnimationContext animationContext)
        {
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
    }
}
