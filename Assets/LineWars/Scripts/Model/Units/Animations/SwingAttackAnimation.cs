
using UnityEngine;

namespace LineWars.Model
{
    public class SwingAttackAnimation : UnitAnimation
    {
        [SerializeField] private SimpleEffect slashEffect;
        [SerializeField] private GameObject effectPosition;

        public override void Execute(AnimationContext context)
        {
            if(slashEffect == null)
            {
                Debug.LogWarning("Slash Effect is null!");
                EndAnimation();
                return;
            }
            if(effectPosition == null)
            {
                Debug.LogWarning("Effect Position is null!");
                EndAnimation();
                return;
            }
            var effect = Instantiate(slashEffect, effectPosition.transform.position, Quaternion.identity);
            effect.Ended += OnEffectEnd;
            StartAnimation();

            void OnEffectEnd()
            {
                EndAnimation();
                if (effect != null)
                    effect.Ended -= OnEffectEnd;    
            }
        }
    }
}
