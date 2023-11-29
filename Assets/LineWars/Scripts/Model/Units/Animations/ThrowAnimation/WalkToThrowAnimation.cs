using UnityEngine;

namespace LineWars.Model
{
    public class WalkToThrowAnimation : UnitAnimation
    {
        [SerializeField, Range(0, 1)] private float movementRange = 0.7f;
        [SerializeField] private float speed = 7f;
        private Vector2 destination;
        public override void Execute(AnimationContext context)
        {
            var moveDirection = (context.TargetUnit.transform.position - ownerUnit.transform.position) * movementRange;
            destination = transform.position + moveDirection;
            IsPlaying = true;
        }

        private void Update()
        {
            if (!IsPlaying)
                return;

            Move();
            CheckIsDestinationReached();
        }

        private void Move()
        {
            ownerUnit.transform.position = Vector2.MoveTowards(ownerUnit.transform.position, destination, speed * Time.deltaTime);
        }

        private void CheckIsDestinationReached()
        {
            if((Vector2)ownerUnit.transform.position == destination)
            {
                IsPlaying = false;
            }
        }
    }
}
