using JetBrains.Annotations;
using System;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoBlockAction :
        MonoUnitAction<BlockAction<Node, Edge, Unit>>,
        IBlockAction<Node, Edge, Unit>
    {
        [SerializeField] private bool initialProtection = false;
        [SerializeField] private IntModifier initialContrAttackDamageModifier;
        public IntModifier InitialContrAttackDamageModifier => initialContrAttackDamageModifier;
        public bool InitialProtection => initialProtection;

        public bool Protection => Action.Protection;
        public IntModifier ContrAttackDamageModifier => Action.ContrAttackDamageModifier;
        public bool IsBlocked => Action.IsBlocked;
        public event Action<bool, bool> CanBlockChanged;

        public bool CanBlock() => Action.CanBlock();
        public void EnableBlock() => Action.EnableBlock();

        public bool CanContrAttack(Unit enemy) => Action.CanContrAttack(enemy);
        public void ContrAttack(Unit enemy) => Action.CanContrAttack(enemy);

        protected override BlockAction<Node, Edge, Unit> GetAction()
        {
            var action = new BlockAction<Node, Edge, Unit>(Unit,
                InitialContrAttackDamageModifier,
                InitialProtection);
            action.CanBlockChanged += (before, after) => CanBlockChanged?.Invoke(before, after);
            return action;
        }
        
        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}