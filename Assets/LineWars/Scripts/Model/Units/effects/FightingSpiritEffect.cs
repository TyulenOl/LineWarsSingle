using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public class FightingSpiritEffect<TNode, TEdge, TUnit> : 
        Effect<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override EffectType EffectType => EffectType.FightingSpirit;

        private int powerBonus;
        private int _givenPower;

        private int givenPower
        {
            get => _givenPower;
            set
            {
                var prevPower = _givenPower;
                _givenPower = value;
                if (_givenPower <= 0 && isActive)
                    isActive = false;
                if (_givenPower > 0 && !isActive)
                    isActive = true;
                InvokeCharacteristicsChanged(this,
                    EffectCharecteristicType.Power,
                    prevPower,
                    _givenPower);
            }
        }

        public FightingSpiritEffect(TUnit targetUnit, int powerBonus) : base(targetUnit)
        {
            this.powerBonus = powerBonus;
            characteristics[EffectCharecteristicType.Power] = () => givenPower;
            isActive = false;
        }

        private HashSet<TUnit> collectedUnits = new();
        private HashSet<TNode> subscribedNodes = new();

        public override void ExecuteOnEnter()
        {
            TargetUnit.UnitNodeChanged += OnUnitNodeChanged;
            CollectAllUnits();  
            SubscribeNodes();
        }

        public override void ExecuteOnExit()
        {
            TargetUnit.UnitNodeChanged -= OnUnitNodeChanged;
            ClearSubscribedNodes();
            DeleteAllUnits();
        }

        private void CollectUnit(TUnit unit)
        {
            if (unit == TargetUnit)
                return;
            if (unit.OwnerId != TargetUnit.OwnerId)
                return;
            if(collectedUnits.Contains(unit))
            {
                Debug.LogError("Unit already collected");
                return;
            }
            collectedUnits.Add(unit);
            TargetUnit.CurrentPower += powerBonus;
            givenPower += powerBonus;
        }

        private void DeleteUnit(TUnit unit)
        {
            if (unit == TargetUnit)
                return;
            if(unit.OwnerId != TargetUnit.OwnerId)
                return;
            if (!collectedUnits.Contains(unit))
            {
                Debug.LogError("Can't delete not collected unit!");
                return;
            }
            collectedUnits.Remove(unit);
            TargetUnit.CurrentPower -= powerBonus;
            givenPower -= powerBonus;
        }

        private void RecollectUnits()
        {
            ClearSubscribedNodes();
            DeleteAllUnits();
            SubscribeNodes();
            CollectAllUnits();
        }

        private void ClearSubscribedNodes()
        {
            foreach (var node in subscribedNodes)
            {
                node.UnitLeft -= OnUnitLeft;
                node.UnitAdded -= OnUnitAdded;
            }

            subscribedNodes.Clear();
        }

        private void DeleteAllUnits()
        {
            var units = new List<TUnit>(collectedUnits);
            foreach (var unit in units)
            {
                DeleteUnit(unit);
            }
        }

        private void CollectAllUnits()
        {
            var neighbors = TargetUnit.Node
                .GetNeighbors()
                .SelectMany(node => node.Units)
                .Where(unit => unit != TargetUnit);
            foreach(var unit in neighbors)
            {
                CollectUnit(unit);
            }
        }

        private void SubscribeNodes()
        {
            var neighbors = TargetUnit.Node.GetNeighbors();
            foreach (var neighbor in neighbors)
            {
                neighbor.UnitLeft += OnUnitLeft;
                neighbor.UnitAdded += OnUnitAdded;
                subscribedNodes.Add(neighbor);
            }
        }

        private void OnUnitNodeChanged(TUnit unit, TNode _, TNode _1)
        {
            RecollectUnits();
        }

        private void OnUnitLeft(TNode node, TUnit unit)
        {
            DeleteUnit(unit);
        }

        private void OnUnitAdded(TNode node, TUnit unit)
        {
            CollectUnit(unit);
        }
    }
}
