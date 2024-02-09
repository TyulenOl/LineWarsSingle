using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class IllnessEffect<TNode, TEdge, TUnit> :
        TemporaryEffect<TNode, TEdge, TUnit>,
        IStackableEffect, IPowerEffect
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private int powerDebuff;
        private int deductedPower;
        public IllnessEffect(
            TUnit targetUnit, 
            int rounds,
            int powerDebuff) : base(targetUnit, rounds)
        {
            this.powerDebuff = powerDebuff;
        }

        public override EffectType EffectType => EffectType.Illness;

        public int Power => powerDebuff;

        public override void ExecuteOnEnter()
        {
            base.ExecuteOnEnter();
            deductedPower = Mathf.Min(TargetUnit.CurrentPower, powerDebuff);
            TargetUnit.CurrentPower -= powerDebuff;
            Spread();   
        }

        public override void ExecuteOnExit()
        {
            base.ExecuteOnExit();
            TargetUnit.CurrentPower += deductedPower;
        }

        private void Spread()
        {
            foreach(var neighbor in TargetUnit.Node.GetNeighbors())
            {
                if(!neighbor.LeftIsFree && neighbor.LeftUnit.Size == UnitSize.Little)
                {
                    var leftEffect = new IllnessEffect<TNode, TEdge, TUnit>
                        (neighbor.LeftUnit, initialRounds, powerDebuff);
                    neighbor.LeftUnit.AddEffect(leftEffect);
                }
                if(!neighbor.RightIsFree)
                {
                    var rightEffect = new IllnessEffect<TNode, TEdge, TUnit>
                        (neighbor.RightUnit, initialRounds, powerDebuff);
                    neighbor.RightUnit.AddEffect(rightEffect);
                }
            }
        }

        public bool CanStack(IStackableEffect effect)
        {
            return effect is IllnessEffect<TNode, TEdge, TUnit>;
        }

        public void Stack(IStackableEffect effect)
        {
            var illnessEffect = (IllnessEffect<TNode, TEdge, TUnit>)effect;
            if(illnessEffect.Rounds > Rounds || illnessEffect.powerDebuff > powerDebuff)
            {
                Rounds = Mathf.Max(Rounds, illnessEffect.Rounds);
                powerDebuff = Mathf.Max(powerDebuff, illnessEffect.powerDebuff);
                Spread();
            }
        }
    }
}
