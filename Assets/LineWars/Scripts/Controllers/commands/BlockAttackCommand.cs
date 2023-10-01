namespace LineWars.Model
{
    public class BlockAttackCommand<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> :
        AttackCommand<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        public BlockAttackCommand(
            TUnit attacker,
            IAlive defender) : base(attacker, defender)
        {
        }

        public BlockAttackCommand(
            AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> attackAction,
            IAlive alive) : base(attackAction, alive)
        {
        }

        public override string GetLog()
        {
            return $"{Defender} перехватил атаку от {Attacker}";
        }
    }
}