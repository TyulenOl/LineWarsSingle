using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [Tooltip("Восполняет очки действия союзному юниту на свою силу духа")]
    public class MonoUpActionPointsAction :
        MonoUnitAction<UpActionPointsAction<Node, Edge, Unit>>,
        IUpActionPointsAction<Node, Edge, Unit>
    {
        [SerializeField] private UnitAnimation unitAnimation;
        [SerializeField] private SFXData sfx;

        protected override bool NeedAutoComplete => false;
        protected override UpActionPointsAction<Node, Edge, Unit> GetAction()
        {
            return new UpActionPointsAction<Node, Edge, Unit>(Executor);
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

        private void ExecuteInstant(Unit target) 
        {
            if (sfx != null)
            {
                SfxManager.Instance.Play(sfx);
            }
            else
            {
                Debug.LogWarning($"Sfx is null on {gameObject.name}");
            }
            Action.Execute(target);
            Complete();
        }

        private void ExecuteAnimation(Unit target)
        {
            var context = new AnimationContext()
            {
                TargetPosition = target.transform.position
            };
            unitAnimation.Ended.AddListener(OnAnimEnd);
            unitAnimation.Execute(context);
            void OnAnimEnd(UnitAnimation _)
            {
                unitAnimation.Ended.RemoveListener(OnAnimEnd);
                ExecuteInstant(target);
            }
        }

        public IActionCommand GenerateCommand(Unit target)
        {
            return new TargetedUniversalCommand<
                Unit, 
                MonoUpActionPointsAction,
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
