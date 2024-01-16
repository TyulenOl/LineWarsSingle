using System;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [Obsolete]
    public class UnitMovementLogic : MonoBehaviour
    {
        [SerializeField, Min(0.0001f)] private int speed = 1;
        [SerializeField] private MovementFunction movementFunction;

        private Queue<Vector2> targetsQueue;

        private bool isMoving;
        private float movementProgress;
        private float timeToTransition;

        private Vector2 startPosition;
        private Vector2 currentTarget;
        public event Action MovementIsOver;

        private void Awake()
        {
            targetsQueue = new Queue<Vector2>();
            if (movementFunction is null)
            {
                movementFunction = ScriptableObject.CreateInstance<LinearMovementFunction>();
                Debug.LogWarning($"У юнита {name} нет функции передвижения! Была выбрана дефолтная функция");
            }
        }

        private void Update()
        {
            if (!isMoving && targetsQueue.Count != 0)
            {
                startPosition = transform.position;
                currentTarget = targetsQueue.Dequeue();
                timeToTransition = (startPosition - currentTarget).magnitude / speed;
                movementProgress = 0;
                isMoving = true;
            }

            if (isMoving)
            {
                movementProgress += Time.deltaTime / timeToTransition;
                if (movementProgress < 1)
                    transform.position = Vector2.Lerp(
                        startPosition,
                        currentTarget,
                        movementFunction.Calculate(movementProgress)
                    );
                else
                {
                    transform.position = currentTarget;
                    MovementIsOver?.Invoke();
                    isMoving = false;
                }
            }
        }

        public void MoveTo(Vector2 target)
        {
            targetsQueue.Enqueue(target);
        }
    }
}