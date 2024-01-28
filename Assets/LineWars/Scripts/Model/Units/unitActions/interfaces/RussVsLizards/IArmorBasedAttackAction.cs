using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public interface IArmorBasedAttackAction<TNode, TEdge, TUnit> :
        IUnitAction<TNode, TEdge, TUnit>,
        ITargetedAction<TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
      
    }
}
