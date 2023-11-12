using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class AIBuyLogicData : ScriptableObject
    {
        public abstract AIBuyLogic CreateAILogic(EnemyAI player);
    }

    public abstract class AIBuyLogic
    {
        public abstract void CalculateBuy();
    }
}
