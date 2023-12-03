
using System.Collections;
using UnityEngine;

namespace LineWars.Model
{
    public class SwingAttackAnimation : UnitAnimation
    {
        [SerializeField] private SimpleEffect slashEffect;
        [SerializeField] private GameObject effectPosition;

        public override void Execute(AnimationContext context)
        {
            if (slashEffect == null )
            {
                Debug.LogWarning($"{nameof(slashEffect)} is null on {name}");
                IsPlaying = true;

                StartCoroutine(StopPlayCoroutine());
                return;
            }

            if (effectPosition == null)
            {
                Debug.LogWarning($"{nameof(effectPosition)} is null on {name}");
                IsPlaying = true;
                
                StartCoroutine(StopPlayCoroutine());
                return;
            }
            
            var effect = Instantiate(slashEffect, effectPosition.transform.position, Quaternion.identity);
            effect.Ended += OnEffectEnd;
            IsPlaying = true;

            void OnEffectEnd()
            {
                IsPlaying = false;
                if (effect != null)
                    effect.Ended -= OnEffectEnd;    
            }

            // кастылище
            IEnumerator StopPlayCoroutine()
            {
                yield return null;
                IsPlaying = false;
            }
        }
    }
}
