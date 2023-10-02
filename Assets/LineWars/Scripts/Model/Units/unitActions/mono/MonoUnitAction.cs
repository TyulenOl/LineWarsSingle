using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(Unit))]
    public abstract class MonoUnitAction: MonoExecutorAction,
        IUnitAction<Node, Edge, Unit, Owned, BasePlayer, Nation>
    {
        protected Unit Unit => (Unit)Executor;

        private UnitAction<Node, Edge, Unit, Owned, BasePlayer, Nation> UnitAction
            => (UnitAction<Node, Edge, Unit, Owned, BasePlayer, Nation>) ExecutorAction;

        public Unit MyUnit => UnitAction.MyUnit;
        public uint GetPossibleMaxRadius() => UnitAction.GetPossibleMaxRadius();
    }
}