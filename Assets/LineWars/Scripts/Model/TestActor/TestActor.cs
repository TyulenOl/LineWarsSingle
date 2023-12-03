using UnityEngine;

namespace LineWars.Model
{
    public class TestActor : BasePlayer
    {
        private StateMachine stateMachine;

        [SerializeField] private int artilleryUnits;
        [SerializeField] private int fightUnits;
        [SerializeField] private int scoutUnits;

        [Header("Units Left")]
        [ReadOnlyInspector] public int ArtilleryLeft;
        [ReadOnlyInspector] public int FightLeft;
        [ReadOnlyInspector] public int ScoutLeft;

        #region Attributes
        public int ArtilleryUnits => artilleryUnits;
        public int FightUnits => fightUnits;
        public int ScoutUnits => scoutUnits;
        #endregion

        public override void ExecuteTurn(PhaseType phaseType)
        {
            Debug.Log($"TestActor {Id} is starting {phaseType} Turn");
            InvokeTurnStarted(phaseType);
            switch(phaseType)
            {
                case PhaseType.Buy:
                    ExecuteBuy();
                    break;
                case PhaseType.Artillery:
                    ExecuteArtillery();
                    break;
                case PhaseType.Fight:
                    ExecuteFight();
                    break;
                case PhaseType.Scout:
                    ExecuteScout();
                    break;
                case PhaseType.Replenish:
                    ExecuteReplenish();
                    break;
            }
            Debug.Log($"TestActor is ending {phaseType} Turn");
            InvokeTurnEnded(phaseType);
        }

        private void ExecuteFight()
        {
            FightLeft--;
        }

        private void ExecuteScout()
        {
            ScoutLeft--;
        }

        private void ExecuteBuy()
        {
            ArtilleryLeft = ArtilleryUnits;
            FightLeft = FightUnits;
            ScoutLeft = ScoutUnits;
        }

        private void ExecuteArtillery()
        {
            ArtilleryLeft--;
        }

        protected override void ExecuteReplenish()
        {
            base.ExecuteReplenish();
            ArtilleryLeft = ArtilleryUnits;
            FightLeft = FightUnits;
            ScoutLeft = ScoutUnits;
        }

        public override bool CanExecuteTurn(PhaseType phaseType)
        {
            if(!base.CanExecuteTurn(phaseType)) 
                return false;

            switch(phaseType)
            {
                case PhaseType.Buy:
                case PhaseType.Replenish:
                    return true;
                case PhaseType.Fight:
                    return FightLeft > 0;
                case PhaseType.Scout:
                    return ScoutLeft > 0;
                case PhaseType.Artillery:
                    return ArtilleryLeft > 0;
            }

            return false;
        }
    }
}
