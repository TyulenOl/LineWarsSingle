using System;
using UnityEngine;

namespace LineWars.Model
{
    public class MovingAnimation : ITransformAnimation
    {
        public Transform Transform { get; private set; }
        private readonly Vector2 destination;
        private readonly float speed;

        public MovingAnimation(
            Transform transform, 
            Vector2 destination, 
            float speed) 
        {
            this.destination = destination;
            this.speed = speed;
            Transform = transform;
        }

        public event Action<ITransformAnimation> Started;
        public event Action<ITransformAnimation> Ended;

        private bool isRunning;

        public void Start()
        {
            isRunning = true;
            Started?.Invoke(this);
        }

        public void Update()
        {
            if (!isRunning) return;
            var newPosition = Vector2.MoveTowards(Transform.position, destination, speed * Time.deltaTime);
            Transform.position = newPosition;
            if ((Vector2)Transform.position == destination)
                End();

        }

        public void End()
        {
            isRunning = false;
            Ended?.Invoke(this);
        }
    }
}
