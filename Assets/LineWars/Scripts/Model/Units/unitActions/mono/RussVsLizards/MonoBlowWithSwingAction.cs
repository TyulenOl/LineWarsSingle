using UnityEngine;

namespace LineWars.Model
{
    public class MonoBlowWithSwingAction: MonoUnitAction,
        IBlowWithSwingAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        private BlowWithSwingAction<Node, Edge, Unit, Owned, BasePlayer> Action
            => (BlowWithSwingAction<Node, Edge, Unit, Owned, BasePlayer>) ExecutorAction;
        [field: SerializeField] public int InitialDamage { get; private set; }
        protected override ExecutorAction GetAction()
        {
            return new BlowWithSwingAction<Node, Edge, Unit, Owned, BasePlayer>(Unit, this);
        }

        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);

        public ICommand GenerateCommand() => new BlowWithSwingCommand<Node, Edge, Unit, Owned, BasePlayer>(this);

        public int Damage => Action.Damage;
        public bool CanBlowWithSwing() => Action.CanBlowWithSwing();

        public void ExecuteBlowWithSwing()
        {
            Action.ExecuteBlowWithSwing();
        }
    }
}