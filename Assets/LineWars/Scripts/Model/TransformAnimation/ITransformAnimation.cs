using UnityEngine;
using System;

namespace LineWars.Model
{
    public interface ITransformAnimation
    {
        public Transform Transform { get; }
        public event Action<ITransformAnimation> Started;
        public event Action<ITransformAnimation> Ended;

        public void Start();
        public void Update();
        public void End();
    }
}
