using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class GameEvaluator : ScriptableObject
    {
        public abstract int Evaluate(GameProjection projection, BasePlayerProjection player);
    }
}
