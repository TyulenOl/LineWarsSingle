using UnityEngine;

namespace LineWars.Model
{
    public class MonoHealYourselfAction: MonoUnitAction<HealYourselfAction<Node,Edge,Unit>>,
        IHealYourselfAction<Node, Edge, Unit>
    {
        [SerializeField, Min(0)] private int initialHealAmount;
        public int HealAmount => Action.HealAmount;
        protected override HealYourselfAction<Node, Edge, Unit> GetAction()
        {
            return new HealYourselfAction<Node, Edge, Unit>(Executor, initialHealAmount);
        }
        
        public bool CanExecute()
        {
            return Action.CanExecute();
        }

        public void Execute()
        {
            //TODO анимации и звуки
            Action.Execute();
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