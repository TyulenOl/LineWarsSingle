using System;

namespace LineWars.Model
{
    public abstract class TemporaryEffect<TNode, TEdge, TUnit> : Effect<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private int rounds;
        protected int Rounds
        {
            get => rounds;
            set
            {
                rounds = value;
                RoundsChanged?.Invoke(this);
                if(rounds <= 0)
                {
                    TargetUnit.RemoveEffect(this);
                }
            }
        }

        public Action<TemporaryEffect<TNode, TEdge, TUnit>> RoundsChanged;
        protected TemporaryEffect(TUnit targetUnit, int rounds) : base(targetUnit)
        {
            this.rounds = rounds;
        }

        public override void ExecuteOnEnter()
        {
            TargetUnit.UnitReplenished += OnReplenish;
        }

        public override void ExecuteOnExit()
        {
            TargetUnit.UnitReplenished -= OnReplenish;
        }
       
        private void OnReplenish(TUnit unit)
        {
            Rounds--;
        }
    }
}
