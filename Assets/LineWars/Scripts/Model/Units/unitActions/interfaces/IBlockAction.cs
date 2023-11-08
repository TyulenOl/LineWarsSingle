using System;
using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public interface IBlockAction<TNode, TEdge, TUnit>:
        IUnitAction<TNode, TEdge, TUnit>, 
        ISimpleAction
    
        #region Сonstraints
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        #endregion 
    {
        bool IsBlocked { get; }
        bool Protection { get; }
        IntModifier ContrAttackDamageModifier { get; }
        event Action<bool, bool> CanBlockChanged;
        bool CanBlock();
        void EnableBlock();
        bool CanContrAttack([NotNull] TUnit enemy);
        void ContrAttack([NotNull] TUnit enemy);
        
        
        bool ISimpleAction.CanExecute() => CanBlock();
        void ISimpleAction.Execute() => Execute();

        IActionCommand ISimpleAction.GenerateCommand()
        {
            return new BlockCommand<TNode, TEdge, TUnit>(this);
        }
    }
}