using UnityEngine;

namespace LineWars.Model
{
    public class ThrowingAnimation : UnitAnimation
    {
        [SerializeField] private float minSpeed = 10f;
        [SerializeField] private float maxSpeed = 10f;

        [SerializeField] private float minRotationInDegrees = 3f;
        [SerializeField] private float maxRotationInDegrees = 3f;

        [SerializeField] private float maxAddititveScale = 0.5f;

        [SerializeField] private AnimationCurve scaleCurve;

        [Header("Sprites")]
        [SerializeField] private SpriteRenderer leftPartSprite;
        [SerializeField] private SpriteRenderer rightPartSprite;

        private Vector2 destination;
        private float totalDistance;
        private float currentSpeed;
        private float currentRotationInDegrees;

        private SpriteRenderer mainSprite;

        private Vector3 initialLocalScale;
        private Quaternion initialRotation;

        public override void Execute(AnimationContext context)
        {
            if (ownerUnit.UnitDirection == UnitDirection.Left)
                mainSprite = leftPartSprite;
            else
                mainSprite = rightPartSprite;

            initialLocalScale = mainSprite.transform.localScale;
            initialRotation = mainSprite.transform.rotation;

            destination = context.TargetNode.transform.position;
            totalDistance = Vector2.Distance(destination, ownerUnit.transform.position);
            currentSpeed = Random.Range(minSpeed, maxSpeed);
            currentRotationInDegrees = Random.Range(minRotationInDegrees, maxRotationInDegrees);

            IsPlaying = true;
        }

        private void Update()
        {
            if (!IsPlaying)
                return;

            Move();
            Rotate();
            ChangeSize();
            CheckIsAtDestination();
        }

        private void Move()
        {
            ownerUnit.transform.position = 
                Vector2.MoveTowards(ownerUnit.transform.position, destination, currentSpeed * Time.deltaTime);
        }
       
        private void ChangeSize()
        {
            var completionRate = Vector2.Distance(ownerUnit.transform.position, destination) / totalDistance;
            var scaleAdditiveAmount = scaleCurve.Evaluate(completionRate);
            mainSprite.transform.localScale = 
                new Vector3(initialLocalScale.x + scaleAdditiveAmount * maxAddititveScale,
                initialLocalScale.x + scaleAdditiveAmount * maxAddititveScale,
                1);
        }

        private void Rotate()
        {
            mainSprite.transform.Rotate(0f, 0f, currentRotationInDegrees);
        }

        private void CheckIsAtDestination()
        {
            if(destination == (Vector2)ownerUnit.transform.position)
            {
                mainSprite.transform.rotation = initialRotation;
                mainSprite.transform.localScale = initialLocalScale;
                IsPlaying = false;
            }
        }
    }
}
