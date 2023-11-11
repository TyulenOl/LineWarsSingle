using System;

namespace LineWars.Model
{
    public class MonoSacrificeForPerunAction :
        MonoUnitAction<SacrificeForPerunAction<Node, Edge, Unit>>,
        ISacrificeForPerunAction<Node, Edge, Unit>
    {

        public bool CanSacrifice(Node node) => Action.CanSacrifice(node);

        public void Sacrifice(Node node)
        {
            //TODO: анимации и звуки
            Action.Sacrifice(node);
            Player.LocalPlayer.AddVisibleNode(node);
            Player.LocalPlayer.RecalculateVisibility();
        }

        protected override SacrificeForPerunAction<Node, Edge, Unit> GetAction()
        {
            return new SacrificeForPerunAction<Node, Edge, Unit>(Unit);
        }
        
        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}