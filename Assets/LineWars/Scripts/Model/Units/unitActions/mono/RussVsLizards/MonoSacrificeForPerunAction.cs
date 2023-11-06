﻿using System;

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
        
        public Type TargetType => typeof(Node);
        public bool IsMyTarget(ITarget target) => target is Node;
        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return new SacrificeForPerunCommand<Node, Edge, Unit>(this, (Node) target);
        }
        
        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}