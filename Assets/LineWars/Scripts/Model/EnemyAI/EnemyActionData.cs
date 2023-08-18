using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace  LineWars.Model
{
    public abstract class EnemyActionData : ScriptableObject
    {
        public abstract bool IsCriteriaMet(EnemyAI enemy, Unit unit);
        public abstract void GetScore(EnemyAI enemy, Unit unit);
        public abstract int GetRequiredActionPoints(EnemyAI enemy, Unit unit);
        public abstract EnemyAction GetAction(EnemyAI enemy, Unit unit);
    }

    public abstract class EnemyAction
    {
        private readonly EnemyAI enemy;
        private readonly Unit unit;
        public EnemyAction(EnemyAI enemy, Unit unit)
        {
            this.enemy = enemy;
            this.unit = unit;
        }

        public abstract void Execute();
        public abstract void GetScore();
        public abstract int GetRequiredActionPoints();
    }
}

