namespace LineWars.Model
{
    public class BlockAttackCommand<TNode, TEdge, TUnit, TOwned, TPlayer> :
        AttackCommand<TNode, TEdge, TUnit, TOwned, TPlayer>
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TOwned, TPlayer>
        #endregion 
    {
        public BlockAttackCommand(
            TUnit attacker,
            IAlive defender) : base(attacker, defender)
        {
        }

        public BlockAttackCommand(
            AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> attackAction,
            IAlive alive) : base(attackAction, alive)
        {
        }

        public override string GetLog()
        {
            return $"{Defender} перехватил атаку от {Attacker}";
        }
    }
}