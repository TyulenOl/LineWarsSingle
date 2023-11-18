using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace LineWars.Model
{
    public enum AnimationResponseType
    {
        MeleeDamaged,
        DistanceDamaged,
        Healed
    }

    public class AnimationResponses : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<AnimationResponseType, UnitAnimation> animations;

        public void Respond(AnimationResponseType responseType, AnimationContext animationContext)
        {
            if (!animations.ContainsKey(responseType))
                return;
            animations[responseType].Execute(animationContext);
        }
    }
}
