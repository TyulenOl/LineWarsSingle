using LineWars.Controllers;
using LineWars.Model.unitActions;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(Unit))]
    public abstract class MonoUnitAction: MonoExecutorAction,
        IUnitAction<Node, Edge, Unit, Owned, BasePlayer, Nation>
    {
        protected UnitAction<Node, Edge, Unit, Owned, BasePlayer, Nation> UnitAction
            => (UnitAction<Node, Edge, Unit, Owned, BasePlayer, Nation>) ExecutorAction;
        public Unit MyUnit => UnitAction.MyUnit;
        public uint GetPossibleMaxRadius() => UnitAction.GetPossibleMaxRadius();
        
        [SerializeField] private SFXData actionSfx;
    }
}