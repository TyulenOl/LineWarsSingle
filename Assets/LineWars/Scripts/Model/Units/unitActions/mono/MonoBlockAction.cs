using JetBrains.Annotations;
using System;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoBlockAction :
        MonoUnitAction<BlockAction<Node, Edge, Unit, Owned, BasePlayer>>,
        IBlockAction<Node, Edge, Unit, Owned, BasePlayer>
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

        protected override BlockAction<Node, Edge, Unit, Owned, BasePlayer> GetAction()
        {
            var action = new BlockAction<Node, Edge, Unit, Owned, BasePlayer>(Unit,
                InitialContrAttackDamageModifier,
                InitialProtection);
            action.CanBlockChanged += (before, after) => CanBlockChanged?.Invoke(before, after);
            return action;
        }

        public ICommandWithCommandType GenerateCommand()
        {
            return new BlockCommand<Node, Edge, Unit, Owned, BasePlayer>(this);
        }

        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, Node, Edge, Unit, Owned, BasePlayer> visitor) => visitor.Visit(this);
    }
}