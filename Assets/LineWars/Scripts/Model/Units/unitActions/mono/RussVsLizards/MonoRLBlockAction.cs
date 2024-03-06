using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public class MonoRLBlockAction :
        MonoUnitAction<RLBlockAction<Node, Edge, Unit>>,
        IRLBlockAction<Node, Edge, Unit>
    {
        [SerializeField] protected SFXList sfxList;

        private IDJ DJ;

        public bool CanBlock() => Action.CanBlock();

        private void Awake()
        {
            DJ = new RandomDJ(0.75f);
        }

        public void EnableBlock()
        {
            Action.EnableBlock();
            if(sfxList != null)
                Executor.PlaySfx(DJ.GetSound(sfxList));
        }

        protected override RLBlockAction<Node, Edge, Unit> GetAction()
        {
            return new RLBlockAction<Node, Edge, Unit>(Executor);
        }
        
        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}