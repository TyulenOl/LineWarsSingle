using System;
using System.Collections.Generic;
namespace LineWars.Model
{
    public class LonelinessEffect<TNode, TEdge, TUnit> : 
        Effect<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override EffectType EffectType => EffectType.Loneliness;
        private TNode executorNode;
        private int powerBonus;

        public LonelinessEffect(TUnit targetUnit, int powerBonus) : base(targetUnit)
        {
            this.powerBonus = powerBonus;
            characteristics[EffectCharecteristicType.Power] = () => this.powerBonus;
            isActive = false;
        }
      
        public override void ExecuteOnEnter()
        {
            TargetUnit.UnitNodeChanged += OnNodeChanged;
            SetExecutorNode(TargetUnit.Node);
        }

        public override void ExecuteOnExit()
        {
            TargetUnit.UnitNodeChanged -= OnNodeChanged;
        }

        private void SetExecutorNode(TNode node)
        {
            if(executorNode != null)
            {
                executorNode.UnitAdded -= OnAnotherUnitEntered;
                executorNode.UnitLeft -= OnAnotherUnitLeft;
            }
            executorNode = node;
            executorNode.UnitAdded += OnAnotherUnitEntered;
            executorNode.UnitLeft += OnAnotherUnitLeft;
            if (TargetUnit.TryGetNeighbour(out var unit))
                Buff();
            else
                Debuff();
        }

        private void OnNodeChanged(TUnit unit, TNode prevNode, TNode currentNode)
        {
            SetExecutorNode(currentNode);
        }

        private void OnAnotherUnitEntered(TNode node, TUnit newUnit)
        {
            if (newUnit == TargetUnit)
                return;
            Buff();
        }

        private void OnAnotherUnitLeft(TNode node, TUnit newUnit)
        {
            if (newUnit == TargetUnit) 
                return;
            Debuff();
        }

        private void Buff()
        {
            if (isActive) return;
            TargetUnit.CurrentPower += powerBonus;
            isActive = true;
        }

        private void Debuff()
        {
            if (!isActive) return;
            TargetUnit.CurrentPower -= powerBonus;
            isActive = false;
        }
    }
}
