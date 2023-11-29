using JetBrains.Annotations;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public class MonoRamAction :
        MonoUnitAction<RamAction<Node, Edge, Unit>>,
        IRamAction<Node, Edge, Unit>
    {
        private MonoMoveAction moveAction;
        protected override bool NeedAutoComplete => false;
        [field: SerializeField] public int InitialDamage { get; private set; }
        [SerializeField] private RamAnimation ramAnimation;

        private int ramResponsesPlayingCount;
        private Node currentNode;

        public override void Initialize()
        {
            base.Initialize();
            moveAction = Executor.GetAction<MonoMoveAction>();
        }

        public int Damage => Action.Damage;

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
            var slowRamEnumerator = Action.SlowRam(currentNode);

            while (slowRamEnumerator.MoveNext())
            {
                var current = slowRamEnumerator.Current;
                //DiedUnit?
                if (current is not MovedUnit movedUnit) continue;

                var currentUnit = (Unit)movedUnit.Unit;
                var currentNode = (Node)movedUnit.DestinationNode;

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

            if (ramResponsesPlayingCount == 0)
                Complete();
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
            var action = new RamAction<Node, Edge, Unit>(Executor, InitialDamage);
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