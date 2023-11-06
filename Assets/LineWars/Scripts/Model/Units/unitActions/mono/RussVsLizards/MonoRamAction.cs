using System;
using System.Collections;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoRamAction :
        MonoUnitAction<RamAction<Node, Edge, Unit>>,
        IRamAction<Node, Edge, Unit>
    {
        private MonoMoveAction moveAction;
        [field: SerializeField] public int InitialDamage { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            moveAction = Unit.GetUnitAction<MonoMoveAction>();
        }
        
        public int Damage => Action.Damage;

        public bool CanRam(Node node)
        {
            return Action.CanRam(node);
        }

        public void Ram(Node node)
        {
            //TODO: анимации и звуки
            StartCoroutine(RamCoroutine(node));
        }

        // Чтобы работать с анимациями
        public IEnumerator RamCoroutine(Node node)
        {
            var moved = false;

            var slowRamEnumerator = Action.SlowRam(node);
            while (slowRamEnumerator.MoveNext())
            {
                var current = slowRamEnumerator.Current;
                if (current is MovedUnit movedUnit)
                {
                    var unit = (Unit) movedUnit.Unit;
                    var monoMoveAction = unit.GetUnitAction<MonoMoveAction>();
                    monoMoveAction.MoveAnimationEnded += OnAnimationEnded;
                    moved = true;

                    while (moved)
                        yield return null;

                    void OnAnimationEnded()
                    {
                        moved = false;
                        monoMoveAction.MoveAnimationEnded -= OnAnimationEnded;
                    }
                }
            }
            Player.LocalPlayer.RecalculateVisibility();
        }

        public Type TargetType => typeof(Node);
        public bool IsMyTarget(ITarget target) => target is Node;
        public ICommandWithCommandType GenerateCommand(ITarget target) => GenerateCommand((Node) target);
        public ICommandWithCommandType GenerateCommand(Node target) => new RamCommand<Node, Edge, Unit>(this, target);
        public bool CanExecute(Node target) => CanRam(target);
        public void Execute(Node target) => Ram(target);

        protected override RamAction<Node, Edge, Unit> GetAction()
        {
            var action = new RamAction<Node, Edge, Unit>(Unit, InitialDamage);
            return action;
        }

        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}