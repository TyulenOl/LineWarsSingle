using UnityEngine.Events;
using UnityEngine;
using System.Collections;

namespace LineWars.Model
{
    public class PerunAnimation : UnitAnimation
    {
        [SerializeField] private float deathTimeInSeconds = 0.6f;
        [Header("Shake Settings")]
        [SerializeField] private float initialShakeMagnitude = 0.3f;
        [SerializeField] private float additiveShakeMagnitude = 0.3f;
        [SerializeField] private float shakePauseInSeconds = 0.04f;
        [SerializeField] private AnimationCurve additiveShakeCurve;
        [Header("Transparent Settings")]
        [SerializeField] private AnimationCurve transparencyCurve;
        [SerializeField] private SpriteRenderer leftSprite;
        [SerializeField] private SpriteRenderer rightSprite;
        [Header("Lightning Settings")]
        [SerializeField] private float pauseBeforeLightningInSeconds = 0.1f;
        [SerializeField] private float lightningTimeInSeconds = 0.5f;
        [SerializeField] private GameObject lightningPrefab;

        private SpriteRenderer mainSprite;
        private Vector2 startPosition;
        public UnityEvent<PerunAnimation> UnitDead;
        private Node targetNode;

        private float shakeMagnitude;
        private float passedTime;

        protected void Start()
        {
            shakeMagnitude = initialShakeMagnitude;
            if (ownerUnit.UnitDirection == UnitDirection.Left)
                mainSprite = leftSprite;
            else
                mainSprite = rightSprite;
        }

        public override void Execute(AnimationContext context)
        {
            targetNode = context.TargetNode;
            IsPlaying = true;
            shakeMagnitude = initialShakeMagnitude;
            startPosition = mainSprite.transform.position;
            StartCoroutine(ShakesCoroutine());
        }

        private IEnumerator ShakesCoroutine()
        {
            while (true)
            {
                var randomDirection = Random.insideUnitCircle * shakeMagnitude;
                mainSprite.transform.position = startPosition + randomDirection;
                shakeMagnitude += additiveShakeCurve.Evaluate(passedTime / deathTimeInSeconds) * additiveShakeMagnitude;
                yield return new WaitForSeconds(shakePauseInSeconds);
                passedTime += shakePauseInSeconds;

                mainSprite.color = mainSprite.color.WithAlpha(1f - transparencyCurve.Evaluate(passedTime / deathTimeInSeconds));
                if(passedTime > deathTimeInSeconds)
                {
                    yield return new WaitForSeconds(pauseBeforeLightningInSeconds);
                    UnitDead.Invoke(this);
                    Strike();
                    yield break;
                }
            }
        }

        private void Strike()
        {
            var lightningInstance = Instantiate(lightningPrefab, targetNode.transform.position, Quaternion.identity);
            StartCoroutine(DestroyCoroutine());
            IEnumerator DestroyCoroutine()
            {
                yield return new WaitForSeconds(lightningTimeInSeconds);
                Destroy(lightningInstance);
                IsPlaying = false;
            }
        }
    }
}
