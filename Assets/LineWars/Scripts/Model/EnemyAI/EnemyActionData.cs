using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace  LineWars.Model
{
    public abstract class EnemyActionData : ScriptableObject
    {
        public abstract void AddAllPossibleActions(List<EnemyAction> list, EnemyAI basePlayer, IExecutor executor);
    }

    public abstract class EnemyAction : IComparable
    {
        protected readonly EnemyAI basePlayer;
        protected readonly IExecutor executor;
        protected float score;

        public event Action<EnemyAction> ActionCompleted;
        
        public IExecutor Executor => executor;
        public float Score => score;
        public EnemyAction(EnemyAI basePlayer, IExecutor executor)
        {
            this.basePlayer = basePlayer;
            this.executor = executor;
        }

        public abstract void Execute();

        protected void InvokeActionCompleted()
        {
            ActionCompleted?.Invoke(this);    
        }
        
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (!(obj is EnemyAction enemyAction)) throw new ArgumentException("Object is not EnemyAction");
            return score.CompareTo(enemyAction.Score);
        }
    }
}

