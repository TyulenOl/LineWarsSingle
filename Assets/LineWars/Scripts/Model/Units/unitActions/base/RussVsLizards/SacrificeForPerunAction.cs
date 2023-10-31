using System;
using System.Linq;

namespace LineWars.Model
{
    public class SacrificeForPerunAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
            UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>,
            ISacrificeForPerunAction<TNode, TEdge, TUnit, TOwned, TPlayer>

        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion

    {
        public SacrificeForPerunAction(TUnit executor) : base(executor)
        {
        }
        
        public override CommandType CommandType => CommandType.SacrificePerun;
        
        public bool CanSacrifice(TNode node)
        {
            return ActionPointsCondition()
                   && node.Owner != Executor.Owner;
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
            return new SacrificeForPerunCommand<TNode, TEdge, TUnit, TOwned, TPlayer>(this, (TNode) target);
        }
        
        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, TNode, TEdge, TUnit, TOwned, TPlayer> visitor) => visitor.Visit(this);
    }
}