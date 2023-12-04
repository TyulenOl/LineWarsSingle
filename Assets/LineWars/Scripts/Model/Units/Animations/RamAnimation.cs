using LineWars.Model;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    public class RamAnimation : UnitAnimation
    {
        public UnityEvent<RamAnimation> Rammed;
        [Header("Prepare Settings")]
        [SerializeField] private float prepareDistance =  3;
        [SerializeField] private float prepareSpeed = 3;

        [Header("Ram Settings")]
        [SerializeField] private float ramSpeed = 30;
        [SerializeField, Range(0f, 1f)] private float distanceToPushTargets = 0.8f;

        private bool isRamming;
        private Vector2 destination;
        private Vector2 startPosition;

        private Node targetNode;
        public override void Execute(AnimationContext context)
        {
            destination = context.TargetNode.Position;
            startPosition = ownerUnit.transform.position;

            targetNode = context.TargetNode;
            StartAnimation();
        }

        private void Update()
        {
            if (!IsPlaying)
                return;

            if (isRamming)
                Ram();
            else
                Prepare();
        }

        private void Ram()
        {
            var newPosition = Vector2.MoveTowards(ownerUnit.transform.position, destination, ramSpeed * Time.deltaTime);
            ownerUnit.transform.position = newPosition;
            var wholeDistance = Vector2.Distance(startPosition, destination);
            var currentDistance = Vector2.Distance(startPosition, newPosition);
            if (currentDistance / wholeDistance >= distanceToPushTargets)
                Rammed.Invoke(this);    

            if (newPosition == destination)
            {
                EndAnimation();
                isRamming = false;
            }
        }
        private void Prepare()
        {
            var moveDirection = (startPosition - destination).normalized;
            ownerUnit.transform.Translate(moveDirection * prepareSpeed * Time.deltaTime);

            if (Vector2.Distance(ownerUnit.transform.position, startPosition) >= prepareDistance)
                isRamming = true;
        }

    }
}
