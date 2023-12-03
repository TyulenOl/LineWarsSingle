
using UnityEngine;

namespace LineWars.Model
{
    public class SwingAttackAnimation : UnitAnimation
    {
        [SerializeField] private SimpleEffect slashEffect;
        [SerializeField] private GameObject effectPosition;

        public override void Execute(AnimationContext context)
        {
            var effect = Instantiate(slashEffect, effectPosition.transform.position, Quaternion.identity);
            effect.Ended += OnEffectEnd;
            IsPlaying = true;

            void OnEffectEnd()
            {
                IsPlaying = false;
                if (effect != null)
                    effect.Ended -= OnEffectEnd;    
            }
        }
    }
}
