using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public class MonoBlowWithSwingAction :
        MonoUnitAction<BlowWithSwingAction<Node, Edge, Unit>>,
        IBlowWithSwingAction<Node, Edge, Unit>
    {
        [field: SerializeField] public int InitialDamage { get; private set; }

        [SerializeField] private SFXList attackReactionSounds;
        [SerializeField] private SFXData attackSound;

        private IDJ dj;
        
        public int Damage => Action.Damage;

        private void Awake()
        {
            dj = new RandomDJ(1);
        }

        public bool IsAvailable(Unit target)
        {
            return Action.IsAvailable(target);
        }

        public void Execute(Unit target)
        {
            //TODO: анимации и звуки
            Executor.PlaySfx(attackSound);
            Action.Execute(target);
            Executor.PlaySfx(dj.GetSound(attackReactionSounds));
        }

        protected override BlowWithSwingAction<Node, Edge, Unit> GetAction()
        {
            return new BlowWithSwingAction<Node, Edge, Unit>(Executor, InitialDamage);
        }

        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) =>
            visitor.Visit(this);
    }
}