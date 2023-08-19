using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace  LineWars.Model
{
    public abstract class EnemyActionData : ScriptableObject
    {
        public abstract void AddAllPossibleActions(List<EnemyAction> list, EnemyAI enemy, IExecutor executor);
    }

    public abstract class EnemyAction : IComparable
    {
        private readonly EnemyAI enemy;
        private readonly IExecutor executor;
        private readonly float score;
        public IExecutor Executor => executor;
        public float Score => score;
        public EnemyAction(EnemyAI enemy, IExecutor executor)
        {
            this.enemy = enemy;
            this.executor = executor;
            score = GetScore();
        }

        public abstract void Execute();
        protected abstract float GetScore();
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (!(obj is EnemyAction enemyAction)) throw new ArgumentException("Object is not EnemyAction");
            return score.CompareTo(enemyAction.Score);
        }
    }
}

