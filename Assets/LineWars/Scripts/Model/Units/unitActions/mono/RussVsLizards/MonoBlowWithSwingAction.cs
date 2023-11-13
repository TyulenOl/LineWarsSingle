using UnityEngine;

namespace LineWars.Model
{
    public class MonoBlowWithSwingAction: 
        MonoUnitAction<BlowWithSwingAction<Node, Edge, Unit>>,
        IBlowWithSwingAction<Node, Edge, Unit>
    {
        [field: SerializeField] public int InitialDamage { get; private set; }
        public int Damage => Action.Damage;
        public bool CanBlowWithSwing() => Action.CanBlowWithSwing();

        public void ExecuteBlowWithSwing()
        {
            //TODO: анимации и звуки
            Action.ExecuteBlowWithSwing();
        }
        
        protected override BlowWithSwingAction<Node, Edge, Unit> GetAction()
        {
            return new BlowWithSwingAction<Node, Edge, Unit>(Unit, InitialDamage);
        }
        
        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}