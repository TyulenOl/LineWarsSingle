using JetBrains.Annotations;

namespace LineWars.Model
{
    public class HealYourSelfCommand<TNode, TEdge, TUnit> :
        ActionCommand<TUnit, IHealYourselfAction<TNode, TEdge, TUnit>>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private int healAmount;
        public HealYourSelfCommand([NotNull] TUnit executor) : base(executor)
        {
            healAmount = Action.HealAmount;
        }

        public HealYourSelfCommand([NotNull] IHealYourselfAction<TNode, TEdge, TUnit> action) : base(action)
        {
            healAmount = Action.HealAmount;
        }

        public override void Execute()
        {
            Action.Execute();
        }

        public override bool CanExecute()
        {
            return Action.CanExecute();
        }

        public override string GetLog()
        {
            return $"Юнит {Executor} похилился на {healAmount} HP";
        }
    }
}