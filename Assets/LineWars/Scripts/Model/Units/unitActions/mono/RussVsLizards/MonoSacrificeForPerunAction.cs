using System;

namespace LineWars.Model
{
    public class MonoSacrificeForPerunAction :
        MonoUnitAction<SacrificeForPerunAction<Node, Edge, Unit, Owned, BasePlayer>>,
        ISacrificeForPerunAction<Node, Edge, Unit, Owned, BasePlayer>
    {

        public bool CanSacrifice(Node node) => Action.CanSacrifice(node);

        public void Sacrifice(Node node)
        {
            //TODO: анимации и звуки
            Action.Sacrifice(node);
            Player.LocalPlayer.AddVisibleNode(node);
            Player.LocalPlayer.RecalculateVisibility();
        }

        protected override SacrificeForPerunAction<Node, Edge, Unit, Owned, BasePlayer> GetAction()
        {
            return new SacrificeForPerunAction<Node, Edge, Unit, Owned, BasePlayer>(Unit);
        }

        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);

        public Type TargetType => typeof(Node);
        public bool IsMyTarget(ITarget target) => target is Node;
        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return new SacrificeForPerunCommand<Node, Edge, Unit, Owned, BasePlayer>(this, (Node) target);
        }
    }
}