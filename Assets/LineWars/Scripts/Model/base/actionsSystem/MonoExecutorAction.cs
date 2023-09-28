using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class MonoExecutorAction: MonoBehaviour
    {
        [SerializeField] protected IntModifier actionModifier;
        
        public IntModifier ActionModifier => actionModifier;

        public abstract ExecutorAction GetAction(IReadOnlyExecutor executor);
    }
}