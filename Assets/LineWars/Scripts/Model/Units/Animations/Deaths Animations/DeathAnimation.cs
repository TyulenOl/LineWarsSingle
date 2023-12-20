using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class DeathAnimation : UnitAnimation
    {
        [SerializeField] private List<GameObject> uiElements;
        [SerializeField] private SpriteRenderer leftSprite;
        [SerializeField] private SpriteRenderer rightSprite;

        [SerializeField] private float animationTime = 0.7f;
        [SerializeField] private float maxDistance = 5;
        [SerializeField] private AnimationCurve distanceCurve;
        [SerializeField] private AnimationCurve alphaCurve;
        [SerializeField] private Vector2 moveDirection = new Vector2(0, -1);

        private SpriteRenderer mainSprite;
        private float currentTime;
        private Vector2 startPosition;

        private void Start()
        {
            moveDirection = moveDirection.normalized;
        }

        public override void Execute(AnimationContext context)
        {
            if (ownerUnit.UnitDirection == UnitDirection.Left)
                mainSprite = leftSprite;
            else
                mainSprite = rightSprite;
            StartAnimation();
            startPosition = ownerUnit.transform.position;
            foreach(var element in uiElements) 
                element.SetActive(false);
        }

        private void Update()
        {
            if(!IsPlaying) return;

            currentTime += Time.deltaTime;
            if(currentTime >= animationTime)
            {
                EndAnimation();
                return;
            }

            var currentDistance = distanceCurve.Evaluate(currentTime / animationTime) * maxDistance;
            ownerUnit.transform.position = startPosition + currentDistance * moveDirection;

            var currentAlpha = alphaCurve.Evaluate(currentTime / animationTime);
            mainSprite.color = mainSprite.color.WithAlpha(currentAlpha);
        }
    }
}
