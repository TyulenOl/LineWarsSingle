using LineWars.Model;

namespace LineWars
{
    public class ArmorBasedAttackAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IArmorBasedAttackAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType => CommandType.ArmorBasedAttack;
        
        private bool listenChangeArmor;
        private bool listenChangePower;
        
        private int startRoundAddedArmor;
        
        public ArmorBasedAttackAction(TUnit executor) : base(executor)
        {
            // startRoundAddedArmor = executor.CurrentArmor;
            //
            // listenChangeArmor = true;
            // listenChangePower = true;
            //
            // executor.CurrentPower = 0;
            //
            // executor.UnitReplenished += ExecutorOnUnitReplenished;
            // executor.UnitPowerChanged += ExecutorOnUnitPowerChanged;
            // executor.UnitArmorChanged += ExecutorOnUnitArmorChanged;
        }

        private void ExecutorOnUnitReplenished(TUnit unit)
        {
            //Executor.CurrentArmor += startRoundAddedArmor;
        }

        private void ExecutorOnUnitPowerChanged(TUnit unit, int before, int after)
        {
            if (!listenChangePower)
                return;
            
            var dif = after - before;
            startRoundAddedArmor += dif;
        }

        private void ExecutorOnUnitArmorChanged(TUnit unit, int before, int after)
        {
            if (!listenChangePower)
                return;
            
            
        }

        public void Execute(TUnit target)
        {
            target.DealDamageThroughArmor(Executor.CurrentArmor);
            Executor.CurrentArmor = 0;
            CompleteAndAutoModify();
        }

        public bool IsAvailable(TUnit target)
        {
            return target != null &&
                ActionPointsCondition() &&
                Executor.CurrentArmor > 0 &&
                target.OwnerId != Executor.OwnerId &&
                Executor.Node.GetLine(target.Node) != null;
        }

        public IActionCommand GenerateCommand(TUnit target)
        {
            return new TargetedUniversalCommand<TUnit, ArmorBasedAttackAction<TNode, TEdge, TUnit>, TUnit>(
                this,
                target);
        }

        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor)
        {
            visitor.Visit(this);
        }

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
