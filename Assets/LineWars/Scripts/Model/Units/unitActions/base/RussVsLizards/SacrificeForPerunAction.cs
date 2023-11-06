using System;
using System.Linq;

namespace LineWars.Model
{
    public class SacrificeForPerunAction<TNode, TEdge, TUnit> :
            UnitAction<TNode, TEdge, TUnit>,
            ISacrificeForPerunAction<TNode, TEdge, TUnit>

        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public SacrificeForPerunAction(TUnit executor) : base(executor)
        {
        }
        
        public override CommandType CommandType => CommandType.SacrificePerun;
        
        public bool CanSacrifice(TNode node)
        {
            return ActionPointsCondition()
                   && node.OwnerId != Executor.OwnerId;
        }

        public void Sacrifice(TNode node)
        {
            var units = new[] {node.LeftUnit, node.RightUnit}
                .Where(x => x != null)
                .Distinct()
                .ToArray();
            
            var damage = MyUnit.CurrentHp / units.Length;

            foreach (var unit in units)
                unit.DealDamageThroughArmor(damage);
            
            
            MyUnit.CurrentHp = 0;
            CompleteAndAutoModify();
        }

        public Type TargetType => typeof(TNode);
        public bool IsMyTarget(ITarget target) => target is TNode;

        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return new SacrificeForPerunCommand<TNode, TEdge, TUnit>(this, (TNode) target);
        }
        
        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
    }
}