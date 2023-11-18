using System.Collections;
using UnityEngine;

namespace LineWars.Model
{
    public class ShakeAnimation : UnitAnimation
    {
        [SerializeField, Min(0)] private float timeInSeconds;
        [SerializeField, Min(0)] private float shakePauseInSeconds;
        [SerializeField, Min(0)] private float shakeMagnitude;

        private Vector2 startPosition;
        private Coroutine shakesCoroutine;

        public override void Execute(AnimationContext context)
        {
            StartCoroutine(StartStopCoroutine());
        }

        private IEnumerator StartStopCoroutine()
        {
            startPosition = transform.position;
            shakesCoroutine = StartCoroutine(ShakesCoroutine());
            yield return new WaitForSeconds(timeInSeconds);
            StopCoroutine(shakesCoroutine);
            transform.position = startPosition;
        }

        private IEnumerator ShakesCoroutine()
        {
            while (true)
            {
                var randomDirection = Random.insideUnitCircle * shakeMagnitude;
                transform.position = startPosition + randomDirection;
                yield return new WaitForSeconds(shakePauseInSeconds);
            }   
        }
    }
}
