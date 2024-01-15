using UnityEngine;

namespace LineWars.Model
{
    public class MovementAnimation : UnitAnimation
    {
        [SerializeField, Min(0.0001f)] private int speed = 1;
        [SerializeField] private MovementFunction movementFunction;

        private bool isMoving;
        private float movementProgress;
        private float timeToTransition;
        private Vector2 targetNode;
        private Vector2 startPosition;

        private void Awake()
        {
            if (movementFunction is null)
            {
                movementFunction = ScriptableObject.CreateInstance<LinearMovementFunction>();
                Debug.LogWarning($"У юнита {name} нет функции передвижения! Была выбрана дефолтная функция");
            }
        }

        public override void Execute(AnimationContext context)
        {
            targetNode = context.TargetNode.transform.position;
            startPosition = ownerUnit.transform.position;
            movementProgress = 0;
            timeToTransition = (startPosition - targetNode).magnitude / speed;
            isMoving = true;
            StartAnimation();
        }

        private void Update()
        {
            if (!isMoving) return;
            movementProgress += Time.deltaTime / timeToTransition;
            if (movementProgress < 1)
                transform.position = Vector2.Lerp(
                    startPosition,
                    targetNode,
                    movementFunction.Calculate(movementProgress)
                );
            else
            {
                transform.position = targetNode;
                isMoving = false;
                EndAnimation();
            }
        }
    }
}
