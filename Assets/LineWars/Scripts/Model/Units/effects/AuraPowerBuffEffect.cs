using UnityEngine;
using System.Collections.Generic;

namespace LineWars.Model
{
    public class AuraPowerBuffEffect<TNode, TEdge, TUnit> : Effect<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private List<TNode> subscribedNodes = new();
        private Dictionary<TUnit, Effect<TNode, TEdge, TUnit>> unitEffects = new();
        public override EffectType EffectType => EffectType.AuraPowerBuff;

        public AuraPowerBuffEffect(TUnit unit) : base(unit)
        {
            
        }

        public override void ExecuteOnEnter()
        {
            TargetUnit.UnitPowerChanged += OnUnitPowerChanged;
            TargetUnit.UnitNodeChanged += OnUnitNodeChanged;
            BuffNewUnits();
            SubscribeNodes();
        }

        public override void ExecuteOnExit()
        {
            TargetUnit.UnitPowerChanged -= OnUnitPowerChanged;
            TargetUnit.UnitNodeChanged -= OnUnitNodeChanged;
            ClearSubscribedNodes();
            DebuffAllUnits();
        }

        private void BuffUnit(TUnit unit) 
        {
            var effect = new PowerBuffEffect<TNode, TEdge, TUnit>(unit, TargetUnit.CurrentPower);
            unit.AddEffect(effect);
            if (unitEffects.ContainsKey(unit))
                Debug.LogError($"Buffing the same unit twice! {unit}");
            unitEffects[unit] = effect;
        }

        private void DebuffUnit(TUnit unit) 
        { 
            if(!unitEffects.ContainsKey(unit))
            {
                Debug.LogError("Can't debuff not buffed unit!");
                return;
            }
            var effect = unitEffects[unit];
            unit.DeleteEffect(effect);
            unitEffects.Remove(unit);
        }

        private void RebuffUnits()
        {
            ClearSubscribedNodes();
            DebuffAllUnits();
            SubscribeNodes();
            BuffNewUnits();
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

        private void DebuffAllUnits()
        {
            var units = new List<TUnit>(unitEffects.Keys);
            foreach (var unit in units)
            {
                DebuffUnit(unit);
            }
        }

        private void BuffNewUnits()
        {
            var neighbors = TargetUnit.Node.GetNeighbors(); 
            foreach (var neighbor in neighbors)
            {
                if (neighbor.LeftUnit != null)
                {
                    BuffUnit(neighbor.LeftUnit);
                }
                if (neighbor.RightUnit != null && neighbor.LeftUnit != neighbor.RightUnit)
                {
                    BuffUnit(neighbor.RightUnit);
                }  
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
            RebuffUnits();
        }

        private void OnUnitLeft(TNode node, TUnit unit) 
        {
            DebuffUnit(unit);
        }

        private void OnUnitAdded(TNode node, TUnit unit) 
        {
            BuffUnit(unit);
        }

        private void OnUnitPowerChanged(TUnit unit, int prevPower, int currentPower)
        {
            RebuffUnits();
        }
    }
}
