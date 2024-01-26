using System;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Blessings/StribogBlessing")]
    public class StribogBlessing: BaseBlessing
    {
        [SerializeField] private int turnsCount;
        private MonoGraph Graph => MonoGraph.Instance;
        public override event Action Completed;
        public override bool CanExecute()
        {
            return true;
        }

        public override void Execute()
        {
            if (turnsCount == 0)
                return;
            foreach (var node in Graph.Nodes)
                Player.AddAdditionalVisibleNode(node);
            Player.RecalculateVisibility();
            Player.TurnEnded += PlayerOnTurnEnded;
        }

        private void PlayerOnTurnEnded(IActor player, PhaseType phaseType)
        {
            
        }
        
        private class StribogAction
        {
            private int counter;
        }
    }
}