using System;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public class MonoRamAction :
        MonoUnitAction<RamAction<Node, Edge, Unit>>,
        IRamAction<Node, Edge, Unit>
    {
        protected override bool NeedAutoComplete => false;
        [SerializeField] private RamAnimation ramAnimation;

        private int ramResponsesPlayingCount;
        private Node currentNode;
        
        public int Damage => Action.Damage;
        public event Action<int> DamageChanged
        {
            add => Action.DamageChanged += value;
            remove => Action.DamageChanged -= value;
        }

        public bool CanRam(Node node)
        {
            return Action.CanRam(node);
        }

        public void Ram(Node node)
        {
            currentNode = node;
            if (ramAnimation == null)
            {
                Executor.transform.position = node.transform.position;
                ExecuteRam();
                return;
            }
            var animContext = new AnimationContext()
            {
                TargetNode = node
            };
            ramAnimation.Execute(animContext);
            ramAnimation.Rammed.AddListener(OnAnimationEnded);
        }

        private void OnAnimationEnded(RamAnimation _)
        {
            ramAnimation.Rammed.RemoveListener(OnAnimationEnded);
            ExecuteRam();
        }

        private void ExecuteRam()
        {
            var units = new List<Unit>()
            {
                currentNode.LeftUnit, currentNode.RightUnit
            };

            foreach (var unit in units)
            {
                if (unit == null)
                    continue;
                if (unit.TryGetComponent<AnimationResponses>(out var responses))
                    responses.TrySetDeathAnimation(AnimationResponseType.RammedDied);
            }

            var slowRamEnumerator = Action.SlowRam(currentNode);

            while (slowRamEnumerator.MoveNext())
            {
                var current = slowRamEnumerator.Current;
                if (current is not MovedUnit movedUnit) continue;

                var currentUnit = (Unit)movedUnit.Unit;
                var currentNode = (Node)movedUnit.DestinationNode;

                if (currentUnit == Executor)
                    continue;
                if (!currentUnit.TryGetComponent(out AnimationResponses responses))
                {
                    currentUnit.transform.position = currentNode.transform.position;
                    continue;
                }

                var animContext = new AnimationContext()
                {
                    TargetNode = (Node)movedUnit.DestinationNode
                };

                if (responses.CanRespond(AnimationResponseType.Rammed))
                {
                    var response = responses.Respond(AnimationResponseType.Rammed, animContext);

                    if (response.IsPlaying)
                    {
                        ramResponsesPlayingCount++;
                        response.Ended.AddListener(OnRespondRamEnded);
                    }
                }
            }

            foreach (var unit in units)
            {
                if (unit == null)
                    continue;
                if (unit.TryGetComponent<AnimationResponses>(out var responses))
                    responses.SetDefaultDeathAnimation();
            }

            if (ramResponsesPlayingCount == 0)
                Complete();

            Player.LocalPlayer.RecalculateVisibility();
        }

        private void OnRespondRamEnded(UnitAnimation animation)
        {
            ramResponsesPlayingCount--;
            animation.Ended.RemoveListener(OnRespondRamEnded);
            if(ramResponsesPlayingCount == 0)
                Complete();
        }

        protected override RamAction<Node, Edge, Unit> GetAction()
        {
            var action = new RamAction<Node, Edge, Unit>(Executor);
            return action;
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