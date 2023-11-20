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
        
        private void OnDestroy()
        {
            CanBlockChanged = null;
        }

        public bool IsBlocked => Action.IsBlocked;
        public event Action<bool, bool> CanBlockChanged;

        public bool CanBlock() => Action.CanBlock();

        private void Awake()
        {
            DJ = new RandomDJ(0.75f);
        }

        public void EnableBlock()
        {
            //TODO: анимации и звуки
            Action.EnableBlock();
            if(sfxList != null)
                Executor.PlaySfx(DJ.GetSound(sfxList));
        }

        protected override RLBlockAction<Node, Edge, Unit> GetAction()
        {
            var action = new RLBlockAction<Node, Edge, Unit>(Executor);
            action.CanBlockChanged += (before, after) => CanBlockChanged?.Invoke(before, after);
            return action;
        }
        
        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}