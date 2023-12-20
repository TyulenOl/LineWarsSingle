using System.Collections;
using UnityEngine;

namespace LineWars.Model
{
    public class ShakeAnimation : UnitAnimation
    {
        [SerializeField, Min(0)] private float timeInSeconds = 0.6f;
        [SerializeField, Min(0)] private float shakePauseInSeconds = 0.04f;
        [SerializeField, Min(0)] private float shakeMagnitude = 0.3f;

        private Vector2 startPosition;
        private Coroutine shakesCoroutine;

        public override void Execute(AnimationContext context)
        {
            StartCoroutine(StartStopCoroutine());
            StartAnimation();
        }

        private IEnumerator StartStopCoroutine()
        {
            startPosition = ownerUnit.transform.position;
            shakesCoroutine = StartCoroutine(ShakesCoroutine());
            yield return new WaitForSeconds(timeInSeconds);
            StopCoroutine(shakesCoroutine);
            ownerUnit.transform.position = startPosition;
            EndAnimation();
        }

        private IEnumerator ShakesCoroutine()
        {
            while (true)
            {
                var randomDirection = Random.insideUnitCircle * shakeMagnitude;
                ownerUnit.transform.position = startPosition + randomDirection;
                yield return new WaitForSeconds(shakePauseInSeconds);
            }   
        }
    }
}
