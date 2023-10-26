﻿using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(Unit))]
    public abstract class MonoUnitAction<TAction> :
        MonoExecutorAction<Unit, TAction>,
        IMonoUnitAction<TAction>
        where TAction : UnitAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        protected Unit Unit => Executor;
        public Unit MyUnit => Action.MyUnit;
        public uint GetPossibleMaxRadius() => Action.GetPossibleMaxRadius();
        public abstract void Accept(IMonoUnitVisitor visitor);
    }
}