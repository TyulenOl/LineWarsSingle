using LineWars.Model;
using UnityEngine;

namespace LineWars.Education
{
    public abstract class ActionWithExecutor: PlayerAction
    {
        [SerializeField] private bool canCancelExecutor;
        public override bool CanCancelExecutor => canCancelExecutor;
    }
}