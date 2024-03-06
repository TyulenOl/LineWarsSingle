using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoHealSacrificeAction :
        MonoUnitAction<HealSacrificeAction<Node, Edge, Unit>>,
        IHealSacrificeAction<Node, Edge, Unit>
    {
        [SerializeField] private UnitAnimation unitAnimation;
        [SerializeField] private SFXData sfx;
        [SerializeField] private SerializedDictionary<UnitType, int> unitToPowerBuff;

        protected override bool NeedAutoComplete => false;
        public IReadOnlyDictionary<UnitType, int> UnitToPowerBuff => unitToPowerBuff;

        protected override HealSacrificeAction<Node, Edge, Unit> GetAction()
        {
            return new HealSacrificeAction<Node, Edge, Unit>(Executor, unitToPowerBuff);
        }
        public bool IsAvailable(Unit target)
        {
            return Action.IsAvailable(target);
        }

        public void Execute(Unit target)
        {
            if (unitAnimation == null)
                ExecuteInstant(target);
            else
                ExecuteAnimation(target);
        }

        private void ExecuteAnimation(Unit target)
        {
            var context = new AnimationContext()
            {
                TargetPosition = target.transform.position,
                TargetUnit = target
            };

            unitAnimation.Ended.AddListener(OnAnimEnd);
            unitAnimation.Execute(context);

            void OnAnimEnd(UnitAnimation _)
            {
                unitAnimation.Ended.RemoveListener(OnAnimEnd);
                ExecuteInstant(target);
            }
        }

        private void ExecuteInstant(Unit target)
        {
            Executor.EnableDamageSfx = false;
            
            if (sfx == null)
                Debug.LogWarning($"sfx is null on {gameObject.name}");
            else
                SfxManager.Instance.Play(sfx);
            
            Action.Execute(target);
            Complete();
        }

        public IActionCommand GenerateCommand(Unit target)
        {
            return new TargetedUniversalCommand<
                Unit,
                MonoHealSacrificeAction,
                Unit>(this, target);
        }

        public override void Accept(IMonoUnitActionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
