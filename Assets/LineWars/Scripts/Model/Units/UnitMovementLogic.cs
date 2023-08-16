using System;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class UnitMovementLogic: MonoBehaviour
    {
        private Queue<Transform> targetsQueue;
        
        private bool isMoving;
        private float movementProgress;
        
        private Vector2 startPosition;
        private Transform currentTarget;

        public event Action<Transform, Transform> TargetChanged;
        public event Action<Transform> MovementIsOver;

        private void Awake()
        {
            targetsQueue = new Queue<Transform>();
        }

        private void Update()
        {
            if (!isMoving && targetsQueue.Count != 0)
            {
                startPosition = transform.position;
                var oldTarget = currentTarget;
                currentTarget = targetsQueue.Dequeue();
                TargetChanged?.Invoke(oldTarget, currentTarget);
                movementProgress = 0;
                isMoving = true;
            }

            if (isMoving)
            {
                movementProgress += Time.deltaTime;
                if (movementProgress < 1)
                    transform.position = Vector2.Lerp(startPosition, currentTarget.position, movementProgress);
                else
                {
                    transform.position = currentTarget.position;
                    MovementIsOver?.Invoke(currentTarget);
                    isMoving = false;
                }
            }
        }
        public void MoveTo(Transform target)
        {
            targetsQueue.Enqueue(target);
        }
    }
}