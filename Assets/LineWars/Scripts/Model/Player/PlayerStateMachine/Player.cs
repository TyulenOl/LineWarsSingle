using System.Collections;
using System.Linq;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars
{
    public partial class Player : BasePlayer
    {
        public static Player LocalPlayer { get; private set; }
        [SerializeField] private PhaseExecutorsData phaseExecutorsData;

        private IReadOnlyCollection<UnitType> potentialExecutors;
        private bool isTurnMade;

        private StateMachine stateMachine;
        private PlayerPhase idlePhase;
        private PlayerPhase artilleryPhase;
        private PlayerPhase fightPhase;
        private PlayerPhase scoutPhase;
        private PlayerBuyPhase buyPhase;
        private PlayerReplenishPhase replenishPhase;
        
        [field: SerializeField] public UnityEvent TurnMade {get; private set;}
        public IReadOnlyCollection<UnitType> PotentialExecutors => potentialExecutors;
        public bool IsTurnMade
        {
            get => isTurnMade;
            private set
            {
                isTurnMade = value;
                if(value)
                    TurnMade.Invoke();
            }
        }


        protected override void Awake()
        {
            base.Awake();
            if (LocalPlayer != null)
                Debug.LogError("More than two players on the scene!");
            else
                LocalPlayer = this;

            
            InitializeStateMachine();
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            OwnedAdded += OnOwnedAdded;
            OwnedRemoved += OnOwnerRemoved;
            CommandsManager.Instance.CommandExecuted.AddListener(OnExecuteCommand);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OwnedAdded -= OnOwnedAdded;
            OwnedRemoved -= OnOwnerRemoved;
            CommandsManager.Instance.CommandExecuted.RemoveListener(OnExecuteCommand);
        }

        private void InitializeStateMachine()
        {
            stateMachine = new StateMachine();
            idlePhase = new PlayerPhase(this, PhaseType.Idle);
            buyPhase = new PlayerBuyPhase(this, PhaseType.Buy);
            artilleryPhase = new PlayerPhase(this, PhaseType.Artillery);
            fightPhase = new PlayerPhase(this, PhaseType.Fight);
            scoutPhase = new PlayerPhase(this, PhaseType.Scout);
            replenishPhase = new PlayerReplenishPhase(this, PhaseType.Replenish);
        }

        private void OnOwnedAdded(Owned owned)
        {
            if(!(owned is ComponentUnit unit)) return;
            unit.ActionPointsChanged.AddListener(ProcessActionPointsChange);
            unit.Died.AddListener(UnitOnDied);
        }

        private void OnOwnerRemoved(Owned owned)
        {
            if(!(owned is ComponentUnit unit)) return;
            unit.ActionPointsChanged.RemoveListener(ProcessActionPointsChange);
            if (!unit.IsDied)
                unit.Died.RemoveListener(UnitOnDied);
        }

        private void ProcessActionPointsChange(int previousValue, int currentValue)
        {
            if (currentValue <= 0 && CurrentPhase != PhaseType.Idle)
                IsTurnMade = true;
        }
        
        public void FinishTurn()
        {
            if(!IsTurnMade) return;
            ExecuteTurn(PhaseType.Idle);
        }

        public IEnumerable<ComponentUnit> GetAllUnitsByPhase(PhaseType phaseType)
        {
            if (phaseExecutorsData.PhaseToUnits.TryGetValue(phaseType, out var value))
            {
                foreach (var myUnit in MyUnits)
                {
                    if (value.Contains(myUnit.Type))
                    {
                        yield return myUnit;
                    }
                }
            }
        }
        
        #region Turns
        public override void ExecuteBuy()
        {
            stateMachine.SetState(buyPhase);
            GameUI.Instance.SetEnemyTurn(false);
        }

        public override void ExecuteArtillery()
        {
            stateMachine.SetState(artilleryPhase);
            GameUI.Instance.ReDrawAllAvailability(GetAllUnitsByPhase(PhaseType.Artillery), true);
            GameUI.Instance.SetEnemyTurn(false);
        }

        public override void ExecuteFight()
        {
            stateMachine.SetState(fightPhase);
            GameUI.Instance.ReDrawAllAvailability(GetAllUnitsByPhase(PhaseType.Fight), true);
            GameUI.Instance.SetEnemyTurn(false);
        }

        public override void ExecuteScout()
        {
            stateMachine.SetState(scoutPhase);
            GameUI.Instance.ReDrawAllAvailability(GetAllUnitsByPhase(PhaseType.Scout), true);
            GameUI.Instance.SetEnemyTurn(false);
        }

        public override void ExecuteIdle()//Exit
        {
            GameUI.Instance.ReDrawAllAvailability(MyUnits, false);
            GameUI.Instance.SetEnemyTurn(true);
            stateMachine.SetState(idlePhase);
        }

        public override void ExecuteReplenish()
        {
            base.ExecuteReplenish();
            stateMachine.SetState(replenishPhase);
        }

        #endregion

        #region Check Turns

        public override bool CanExecuteBuy() => true;

        public override bool CanExecuteArtillery() => CanExecutePhase(PhaseType.Artillery);

        public override bool CanExecuteFight() => CanExecutePhase(PhaseType.Fight);

        public override bool CanExecuteScout() => CanExecutePhase(PhaseType.Scout);

        public override bool CanExecuteReplenish() => true;

        private bool CanExecutePhase(PhaseType phaseType)
        {
            var phaseExecutors = phaseExecutorsData.PhaseToUnits[phaseType];

            foreach (var owned in OwnedObjects)
            {
                if(!(owned is ComponentUnit unit)) continue;
                if(phaseExecutors.Contains(unit.Type) && unit.CurrentActionPoints > 0)
                    return true;
            }

            return false;
        }

        #endregion
        
        private void UnitOnDied(ComponentUnit diedUnit)
        {
            StartCoroutine(UnitOnDiedCoroutine());
            IEnumerator UnitOnDiedCoroutine()
            {
                yield return null;
                RecalculateVisibility();
            }
        }
        private void OnExecuteCommand(IExecutor executor, ITarget target)
        {
            RecalculateVisibility();
        }

        public void RecalculateVisibility(bool useLerp = true)
        {
            var visibilityMap = Graph.GetVisibilityInfo(this);
            foreach (var (node, visibility) in visibilityMap)
            {
                if (useLerp)
                    node.RenderNodeV3.SetVisibilityGradually(visibility ? 1 : 0);
                else
                    node.RenderNodeV3.SetVisibilityInstantly(visibility ? 1 : 0);
            }
        }
    }
}