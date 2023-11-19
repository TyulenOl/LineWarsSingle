using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public class MonoBlowWithSwingAction :
        MonoUnitAction<BlowWithSwingAction<Node, Edge, Unit>>,
        IBlowWithSwingAction<Node, Edge, Unit>
    {
        [field: SerializeField] public int InitialDamage { get; private set; }
        public int Damage => Action.Damage;

        public bool IsAvailable(Unit target)
        {
            return Action.IsAvailable(target);
        }

        public void Execute(Unit target)
        {
            //TODO: анимации и звуки
            Action.Execute(target);
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