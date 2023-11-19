using UnityEngine;

namespace LineWars.Model
{
    public class ThrowingAnimation : UnitAnimation
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float rotationSpeedInDegrees = 3f;
        [SerializeField] private float maxAddititveScale = 0.5f;

        [SerializeField] private AnimationCurve scaleCurve;

        [Header("Sprites")]
        [SerializeField] private SpriteRenderer leftPartSprite;
        [SerializeField] private SpriteRenderer rightPartSprite;

        private Vector2 Destination;
        private float totalDistance;

        private SpriteRenderer mainSprite;
        private float initialXScale;
        private float initialYScale;

        public override void Initialize(Unit ownerUnit)
        {
            base.Initialize(ownerUnit);
            if(ownerUnit.UnitDirection == UnitDirection.Left)
                mainSprite = leftPartSprite;
            else
                mainSprite = rightPartSprite;
            initialXScale = mainSprite.transform.localScale.x;
            initialYScale = mainSprite.transform.localScale.y;
        }
        public override void Execute(AnimationContext context)
        {
            Destination = context.TargetNode.transform.position;
            totalDistance = Vector2.Distance(Destination, ownerUnit.transform.position);
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
                Vector2.MoveTowards(ownerUnit.transform.position, Destination, speed * Time.deltaTime);
        }
       
        private void ChangeSize()
        {
            var completionRate = Vector2.Distance(ownerUnit.transform.position, Destination) / totalDistance;
            var scaleAdditiveAmount = scaleCurve.Evaluate(completionRate);
            mainSprite.transform.localScale = 
                new Vector3(initialXScale + scaleAdditiveAmount * maxAddititveScale,
                initialXScale + scaleAdditiveAmount * maxAddititveScale,
                1);
        }

        private void Rotate()
        {
            var oldZ = ownerUnit.transform.rotation.z;
            mainSprite.transform.Rotate(0f, 0f, oldZ + rotationSpeedInDegrees);
        }

        private void CheckIsAtDestination()
        {
            if(Destination == (Vector2)ownerUnit.transform.position)
            {
                mainSprite.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                mainSprite.transform.localScale = new Vector2(initialXScale, initialYScale);
                IsPlaying = false;
            }
        }
    }
}
