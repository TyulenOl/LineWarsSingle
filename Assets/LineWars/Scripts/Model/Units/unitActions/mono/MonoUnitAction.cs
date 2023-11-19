using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(Unit))]
    public abstract class MonoUnitAction<TAction> :
        MonoExecutorAction<Unit, TAction>,
        IMonoUnitAction<TAction>
        where TAction : UnitAction<Node, Edge, Unit>
    {
        public uint GetPossibleMaxRadius() => Action.GetPossibleMaxRadius();
        public abstract void Accept(IMonoUnitActionVisitor visitor);
        public abstract TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor);
    }
}