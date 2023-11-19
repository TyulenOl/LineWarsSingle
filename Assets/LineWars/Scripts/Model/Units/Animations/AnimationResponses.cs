using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace LineWars.Model
{
    public enum AnimationResponseType
    {
        MeleeDamaged,
        DistanceDamaged,
        ComeTo,
        Throw
    }

    [DisallowMultipleComponent]
    public class AnimationResponses : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<AnimationResponseType, UnitAnimation> animations;

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
    }
}
