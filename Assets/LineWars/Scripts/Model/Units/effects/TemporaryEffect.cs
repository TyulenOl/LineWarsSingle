using System;

namespace LineWars.Model
{
    public abstract class TemporaryEffect<TNode, TEdge, TUnit> : 
        Effect<TNode, TEdge, TUnit>, ITemporaryEffect
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private int rounds;
        protected readonly int initialRounds;
        public int Rounds
        {
            get => rounds;
            protected set
            {
                rounds = value;
                RoundsChanged?.Invoke(this);
                if(rounds <= 0)
                {
                    TargetUnit.RemoveEffect(this);
                }
            }
        }

        public int InitialRounds => initialRounds;

        public Action<TemporaryEffect<TNode, TEdge, TUnit>> RoundsChanged;
        protected TemporaryEffect(TUnit targetUnit, int rounds) : base(targetUnit)
        {
            this.rounds = rounds;
            initialRounds = rounds;
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
