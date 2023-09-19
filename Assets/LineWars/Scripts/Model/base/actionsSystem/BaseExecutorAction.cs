using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class BaseExecutorAction: MonoBehaviour
    {
        [SerializeField] protected IntModifier actionModifier;
        [SerializeField] private SFXData actionSfx;
        
        public IntModifier ActionModifier => actionModifier;

        public SFXData ActionSfx => actionSfx;

        public abstract ExecutorAction GetAction(IExecutor executor);
    }
}