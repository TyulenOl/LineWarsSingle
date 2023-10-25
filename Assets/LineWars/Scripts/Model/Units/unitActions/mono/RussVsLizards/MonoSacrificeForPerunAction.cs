namespace LineWars.Model
{
    public class MonoSacrificeForPerunAction : MonoUnitAction,
        ISacrificeForPerunAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        private SacrificeForPerunAction<Node, Edge, Unit, Owned, BasePlayer> Action
            => (SacrificeForPerunAction<Node, Edge, Unit, Owned, BasePlayer>) ExecutorAction;
        protected override ExecutorAction GetAction() => new SacrificeForPerunAction<Node, Edge, Unit, Owned, BasePlayer>(Unit, this);

        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);

        public bool IsMyTarget(ITarget target)
            => target is Node;

        public ICommand GenerateCommand(ITarget target)
            => new SacrificePerunCommand<Node, Edge, Unit, Owned, BasePlayer>(this, (Node) target);

        public bool CanSacrifice(Node node) => Action.CanSacrifice(node);

        public void Sacrifice(Node node)
        {
            Action.Sacrifice(node);
            Player.LocalPlayer.AddVisibleNode(node);
            Player.LocalPlayer.RecalculateVisibility();
        }
    }
}