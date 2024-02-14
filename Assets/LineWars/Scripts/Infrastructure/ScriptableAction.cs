using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LineWars.Infrastructure
{
    public abstract class ScriptableAction : ScriptableObject
    {
        public abstract void Execute();
    }
}