using System;
using System.Collections;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoRamAction : MonoUnitAction,
        IRamAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        private RamAction<Node, Edge, Unit, Owned, BasePlayer> Action
            => (RamAction<Node, Edge, Unit, Owned, BasePlayer>) ExecutorAction;

        private MonoMoveAction moveAction;
        [field: SerializeField] public int InitialDamage { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            moveAction = Unit.GetUnitAction<MonoMoveAction>();
        }

        protected override ExecutorAction GetAction()
        {
            var action = new RamAction<Node, Edge, Unit, Owned, BasePlayer>(Unit, this);
            return action;
        }

        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);

        public int Damage => Action.Damage;

        public bool CanRam(Node node)
        {
            return Action.CanRam(node);
        }

        public void Ram(Node node)
        {
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
        }

        public Type TargetType => typeof(Node);
        public bool IsMyTarget(ITarget target) => target is Node;

        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return new RamCommand<Node, Edge, Unit, Owned, BasePlayer>(Unit, (Node) target);
        }
    }
}