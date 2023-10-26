using UnityEngine;

namespace LineWars.Model
{
    public class MonoBlowWithSwingAction: 
        MonoUnitAction<BlowWithSwingAction<Node, Edge, Unit, Owned, BasePlayer>>,
        IBlowWithSwingAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        [field: SerializeField] public int InitialDamage { get; private set; }
        public int Damage => Action.Damage;
        public bool CanBlowWithSwing() => Action.CanBlowWithSwing();

        public void ExecuteBlowWithSwing()
        {
            //TODO: анимации и звуки
            Action.ExecuteBlowWithSwing();
        }
        
        protected override BlowWithSwingAction<Node, Edge, Unit, Owned, BasePlayer> GetAction()
        {
            return new BlowWithSwingAction<Node, Edge, Unit, Owned, BasePlayer>(Unit);
        }

        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);

        public ICommandWithCommandType GenerateCommand() => new BlowWithSwingCommand<Node, Edge, Unit, Owned, BasePlayer>(this);
    }
}