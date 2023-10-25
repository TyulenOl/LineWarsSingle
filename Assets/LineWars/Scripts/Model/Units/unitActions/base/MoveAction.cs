using System;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class MoveAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
        UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>, 
        IMoveAction<TNode, TEdge, TUnit, TOwned, TPlayer>
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TOwned, TPlayer>
        #endregion 
    {
        public MoveAction(
            [NotNull] TUnit unit,
            [NotNull] MonoMoveAction data) : base(unit, data)
        {
        }

        public MoveAction(
            [NotNull] TUnit unit,
            [NotNull] MoveAction<TNode, TEdge, TUnit, TOwned, TPlayer> data) : base(unit, data)
        {
        }

        public bool CanMoveTo([NotNull] TNode target, bool ignoreActionPointsCondition = false)
        {
            return MyUnit.Node != target
                   && OwnerCondition()
                   && SizeCondition()
                   && LineCondition()
                   && (ignoreActionPointsCondition || ActionPointsCondition());

            bool SizeCondition()
            {
                return MyUnit.Size == UnitSize.Little && target.AnyIsFree
                       || MyUnit.Size == UnitSize.Large && target.AllIsFree;
            }

            bool LineCondition()
            {
                var line = MyUnit.Node.GetLine(target);
                return line != null
                       && MyUnit.CanMoveOnLineWithType(line.LineType);
            }

            bool OwnerCondition()
            {
                return target.Owner == null
                       || target.Owner == MyUnit.Owner
                       || target.Owner != MyUnit.Owner && target.AllIsFree;
            }
        }

        public void MoveTo([NotNull] TNode target)
        { 
            var startNode = MyUnit.Node;

            if (startNode.LeftUnit == MyUnit)
                startNode.LeftUnit = null;
            if (startNode.RightUnit == MyUnit)
                startNode.RightUnit = null;

            InspectNodeForCallback();
            AssignNewNode();

            
            CompleteAndAutoModify();

            void InspectNodeForCallback()
            {
                if (target.Owner == null)
                {
                    OnCapturingFreeNode();
                    return;
                }

                if (target.Owner != MyUnit.Owner)
                {
                    OnCapturingEnemyNode();
                    if (target.IsBase)
                        OnCapturingEnemyBase();
                }
            }

            void AssignNewNode()
            {
                MyUnit.Node = target;
                if (MyUnit.Size == UnitSize.Large)
                {
                    target.LeftUnit = MyUnit;
                    target.RightUnit = MyUnit;
                }
                else if (target.LeftIsFree && (MyUnit.UnitDirection == UnitDirection.Left ||
                                               MyUnit.UnitDirection == UnitDirection.Right && !target.RightIsFree))
                {
                    target.LeftUnit = MyUnit;
                    MyUnit.UnitDirection = UnitDirection.Left;
                }
                else
                {
                    target.RightUnit = MyUnit;
                    MyUnit.UnitDirection = UnitDirection.Right;
                }

                if (MyUnit.Owner != target.Owner)
                    target.ConnectTo(MyUnit.Owner);
            }
        }

        public override CommandType CommandType => CommandType.Move;

        public Type TargetType => typeof(TNode);
        public bool IsMyTarget(ITarget target) => target is TNode;

        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return new MoveCommand<TNode, TEdge, TUnit, TOwned, TPlayer>(this, (TNode) target);
        }

        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor)
        {
            visitor.Visit(this);
        }

        #region CallBack

        protected virtual void OnCapturingEnemyBase()
        {
        }

        protected virtual void OnCapturingEnemyNode()
        {
        }

        protected virtual void OnCapturingFreeNode()
        {
        }

        #endregion
    }
}